using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalPagesHandler : MonoBehaviour
{
    [Header("Settings")]
    public List<JournalPageButton> jorunalPageButtons; 

    public class JournalPageButton
    {
        public int pageNumber;
        public Button pageButton;
    }
}
