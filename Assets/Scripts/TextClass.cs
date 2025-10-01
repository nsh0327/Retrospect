using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [Serializable]
    public class TextItem
    {
        public string id;
        public string content;
    }
    
    
    [Serializable]

    public class GameTextData
    {
        public List<TextItem> texts;
    }
