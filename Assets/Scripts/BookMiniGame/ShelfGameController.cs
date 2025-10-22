using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BookshelfMiniGame
{
    public class ShelfGameController : MonoBehaviour
    {
        [Header("References")]
        public Transform ShelfLeftAnchor;      // (-9.0, 0, 4)
        public Transform ShelfRightAnchor;     // (10.5, 0, 4)
        public Camera GameplayCamera;
        public List<BookItem> Books;

        [Header("Layout (Fixed)")]
        public float Gap = 0.5f;               // 책 사이 간격
        public float WallGap = 0.5f;           // 좌/우 벽 간격
        public float AnimateSpeed = 12f;

        [Header("Drag Settings")]
        public float DragZOffset = -1f;        // 클릭/드래그 동안 z 오프셋(앞으로: BaseZ + (-1))
        public bool ShuffleAtStart = true;

        public Action OnSolved;

        const float k_ReflowSnap = 0.10f;

        // 상태
        List<int> initialOrder = new List<int>();
        bool isDragging = false;
        bool isLerping = false;
        BookItem selected = null;
        int previewInsertIndex = -1;

        // 선반 축
        Vector3 origin, rightDir;
        float shelfLength;

        void Start()
        {
            // 선반 축/길이
            origin = ShelfLeftAnchor.position;
            Vector3 v = ShelfRightAnchor.position - ShelfLeftAnchor.position;
            rightDir = v.normalized;
            shelfLength = v.magnitude; // 19.5

            // 참조 일관성
            Books = Books.OrderBy(b => b.Id).ToList();

            // 시작 1회 셔플
            if (ShuffleAtStart) ShuffleOnce();
            else initialOrder = Books.Select(b => b.Id).ToList();

            StartCoroutine(ReflowLayoutSmooth(false));
            UpdateSolvedCountUI();
        }

        void Update()
        {
            if (GameplayCamera == null) return;

            if (Input.GetMouseButtonDown(0)) TryBeginDrag();
            else if (Input.GetMouseButton(0) && isDragging && selected != null) ContinueDrag();
            else if (Input.GetMouseButtonUp(0) && isDragging) EndDrag();
        }

        // ---------- 드래그 시작 ----------
        void TryBeginDrag()
        {
            var ray = GameplayCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 200f)) return;

            var book = hit.collider.GetComponentInParent<BookItem>();
            if (book == null || isLerping) return;

            selected = book;
            isDragging = true;

            // 선택 책만 리스트에서 잠시 제외 → 나머지만 왼쪽 정렬
            Books.Remove(book);
            StartCoroutine(ReflowLayoutSmooth(true)); // ignoreSelected = true

            // 눌렀을 때 즉시 앞으로 보이도록 z 오프셋 적용
            Vector3 centerPos = selected.transform.position;
            centerPos.z = selected.BaseZ + DragZOffset;
            selected.transform.position = centerPos;
        }

        // ---------- 드래그 중 ----------
        void ContinueDrag()
        {
            // 선반 평면과 평행한 드래그 평면
            Vector3 up = Vector3.up;
            Vector3 planeNormal = Vector3.Cross(rightDir, up).normalized;
            var dragPlane = new Plane(planeNormal, origin);

            var ray = GameplayCamera.ScreenPointToRay(Input.mousePosition);
            if (!dragPlane.Raycast(ray, out float enter)) return;

            Vector3 hit = ray.GetPoint(enter);

            // '센터' 좌표로 해석
            float xCenter = Vector3.Dot(hit - origin, rightDir);

            // 센터 기준 클램프: [벽간+반폭, 선반길이-벽간-반폭]
            float half = selected.Width * 0.5f;
            float minCenter = WallGap + half;
            float maxCenter = shelfLength - WallGap - half;
            xCenter = Mathf.Clamp(xCenter, minCenter, maxCenter);

            // 드래그 동안: 센터 이동 + z 오프셋 유지, y는 고정
            Vector3 pos = origin + rightDir * xCenter;
            pos.y = selected.BaseY;
            pos.z = selected.BaseZ + DragZOffset;
            selected.transform.position = pos;

            // 현재 센터 기준으로 삽입 위치 계산
            previewInsertIndex = ComputeInsertIndexByCenter(xCenter);
        }

        // ---------- 드래그 종료 ----------
        void EndDrag()
        {
            isDragging = false;
            if (selected == null) return;

            int insertIndex = Mathf.Clamp(previewInsertIndex, 0, Books.Count);
            Books.Insert(insertIndex, selected);

            // 드롭 시: 센터 스냅 + z 복귀(BaseZ), y 고정
            Vector3 target = ComputeTargetPositionOfIndex(insertIndex, selected);
            selected.transform.position = new Vector3(target.x, selected.BaseY, selected.BaseZ);

            selected = null;
            previewInsertIndex = -1;

            StartCoroutine(ReflowLayoutSmooth(false));
            UpdateSolvedCountUI();
            if (IsSolved()) OnSolved?.Invoke();
        }

        // ---------- 삽입 인덱스(센터 기준) ----------
        int ComputeInsertIndexByCenter(float xCenter)
        {
            float run = WallGap; // 왼쪽 모서리 누적, 비교는 센터
            for (int i = 0; i < Books.Count; i++)
            {
                float mid = run + Books[i].Width * 0.5f;
                if (xCenter < mid) return i;
                run += Books[i].Width + Gap;
            }
            return Books.Count;
        }

        // ---------- 목표 좌표(센터 배치) ----------
        Vector3 ComputeTargetPositionOfIndex(int index, BookItem targetBook)
        {
            float run = WallGap;
            for (int i = 0; i < index; i++)
                run += Books[i].Width + Gap;

            float xCenter = run + targetBook.Width * 0.5f;
            Vector3 pos = origin + rightDir * xCenter;
            pos.y = targetBook.BaseY;   // y 고정
            pos.z = targetBook.BaseZ;   // z 복귀 고정
            return pos;
        }

        // ---------- 셔플 / 리셋 ----------
        public void ResetToInitial()
        {
            if (selected != null) return;
            Books = initialOrder.Select(id => Books.First(b => b.Id == id)).ToList();
            StartCoroutine(ReflowLayoutSmooth(false));
            UpdateSolvedCountUI();
        }

        public void ReshuffleNow()
        {
            if (selected != null) return;
            ShuffleOnce();
            StartCoroutine(ReflowLayoutSmooth(false));
            UpdateSolvedCountUI();
        }

        void ShuffleOnce()
        {
            System.Random rng = new System.Random();
            for (int i = Books.Count - 1; i > 0; i--)
            {
                int j = rng.Next(0, i + 1);
                (Books[i], Books[j]) = (Books[j], Books[i]);
            }
            initialOrder = Books.Select(b => b.Id).ToList();
        }

        // ---------- 레이아웃(센터 기준) ----------
        IEnumerator ReflowLayoutSmooth(bool ignoreSelected)
        {
            isLerping = true;

            var target = new Dictionary<BookItem, Vector3>();
            float run = WallGap;

            foreach (var b in Books)
            {
                float xCenter = run + b.Width * 0.5f;
                Vector3 p = origin + rightDir * xCenter;
                p.y = b.BaseY;
                p.z = b.BaseZ;
                target[b] = p;

                run += b.Width + Gap;
            }

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * AnimateSpeed;
                foreach (var b in Books)
                {
                    if (ignoreSelected && selected == b) continue;
                    b.transform.position = Vector3.Lerp(b.transform.position, target[b], t);
                }
                yield return null;
            }

            foreach (var b in Books)
            {
                if (ignoreSelected && selected == b) continue;
                b.transform.position = target[b];
            }

            yield return new WaitForSeconds(k_ReflowSnap);
            isLerping = false;
        }

        // ---------- 정답 ----------
        public int GetSolvedCount()
        {
            int count = 0;
            for (int i = 0; i < Books.Count; i++)
                if (Books[i].Id == (i + 1)) count++;
            return count;
        }

        public bool IsSolved()
        {
            for (int i = 0; i < Books.Count; i++)
                if (Books[i].Id != (i + 1)) return false;
            return true;
        }

        public void UpdateSolvedCountUI()
        {
            Debug.Log($"Solved {GetSolvedCount()}/{Books.Count}");
        }

        public void ApplyInitialOrder()
        {
            if (selected != null) return;
            Books = initialOrder.Select(id => Books.First(b =>  b.Id == id)).ToList();
            StartCoroutine(ReflowLayoutSmooth(false));
            UpdateSolvedCountUI();
        }

        public int GetCorrectCount()
        {
            return GetSolvedCount();
        }
    }
}
