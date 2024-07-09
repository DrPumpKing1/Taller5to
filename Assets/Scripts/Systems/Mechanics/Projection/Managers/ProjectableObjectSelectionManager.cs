using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectableObjectSelectionManager : MonoBehaviour
{
    public static ProjectableObjectSelectionManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private PlayerInteractAlternate playerInteractAlternate;
    [SerializeField] private ProjectionInput projectionInput;

    [Header("Projectable Object Settings")]
    [SerializeField] private List<ProjectableObjectIndexed> projectableObjectsIndexed = new List<ProjectableObjectIndexed>();
    [SerializeField] private ProjectableObjectIndexed selectedProjectableObjectIndexed;

    [Header("Selection Settings")]
    [SerializeField] private int InitialSelectionIndex;
    [SerializeField] private int currentSelectionIndex;

    public static event EventHandler<OnSelectionEventArgs> OnProjectableObjectSelected;
    public static event EventHandler<OnSelectionEventArgs> OnProjectableObjectDeselected;

    public static event EventHandler<OnObjectAddedToInventoryEventArgs> OnObjectAddedToInventory;

    public static event EventHandler OnProjectableObjectSelectionManagerInitialized;

    private bool SelectionInputNext => projectionInput.GetNextProjectableObjectDown();
    private bool SelectionInputPrevious => projectionInput.GetPreviousProjectableObjectDown();
    private bool _1stProjectableObjectInput => projectionInput.Get1stProjectableObjectDown();
    private bool _2ndProjectableObjectInput => projectionInput.Get2ndProjectableObjectDown();
    private bool _3rdProjectableObjectInput => projectionInput.Get3rdProjectableObjectDown();
    private bool _4thProjectableObjectInput => projectionInput.Get4thProjectableObjectDown();

    public List<ProjectableObjectIndexed> ProjectableObjectsIndexed => projectableObjectsIndexed;
    public ProjectableObjectIndexed SelectedProjectableObjectIndexed => selectedProjectableObjectIndexed;

    [Serializable]
    public class ProjectableObjectIndexed
    {
        public int index;
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnObjectAddedToInventoryEventArgs : EventArgs
    {
        public ProjectableObjectIndexed projectableObjectIndexed;
    }

    public class OnSelectionEventArgs : EventArgs
    {
        public ProjectableObjectIndexed projectableObjectIndexed;
    }


    public void OnEnable()
    {
        ProjectableObjectsLearningManager.OnProjectableObjectLearned += ProjectableObjectsLearningManager_OnProjectableObjectLearned;
    }
    public void OnDisable()
    {
        ProjectableObjectsLearningManager.OnProjectableObjectLearned -= ProjectableObjectsLearningManager_OnProjectableObjectLearned;
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {
        InitializeVariables();
    }

    private void Update()
    {
        HandleProjectableObjectSelection();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("There is more than one ProjectableObjectSelectionManager instance, proceding to destroy duplicate");
            Destroy(gameObject);
        }
    }

    private void InitializeVariables()
    {
        InitializeProjectableObjectsIndexed();
        ClampInitialSelectionIndex();

        if (projectableObjectsIndexed.Count > 0)
        {
            SelectProjectableObject(projectableObjectsIndexed[InitialSelectionIndex]);
        }

        currentSelectionIndex = InitialSelectionIndex;

        OnProjectableObjectSelectionManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    private void InitializeProjectableObjectsIndexed()
    {
        int index = 0;

        foreach(ProjectableObjectSO projectableObjectSO in ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned)
        {
            ProjectableObjectIndexed projectableObjectIndexed = new ProjectableObjectIndexed { index = index, projectableObjectSO = projectableObjectSO };
            projectableObjectsIndexed.Add(projectableObjectIndexed);
        }
    }

    private void AddProjectableObjectToIndexedList(ProjectableObjectSO projectableObjectSO)
    {
        int index = projectableObjectsIndexed.Count;

        ProjectableObjectIndexed projectableObjectIndexed = new ProjectableObjectIndexed { index = index, projectableObjectSO = projectableObjectSO};
        projectableObjectsIndexed.Add(projectableObjectIndexed);

        OnObjectAddedToInventory?.Invoke(this, new OnObjectAddedToInventoryEventArgs { projectableObjectIndexed = projectableObjectIndexed });

        CheckIsFirstLearned();
    }

    private void CheckIsFirstLearned()
    {
        if (projectableObjectsIndexed.Count > 1) return;
        
        currentSelectionIndex = 0;
        SelectProjectableObject(projectableObjectsIndexed[currentSelectionIndex]);
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { projectableObjectIndexed = selectedProjectableObjectIndexed });    
    }

    private void ClampInitialSelectionIndex()
    {
        InitialSelectionIndex = projectableObjectsIndexed.Count <= InitialSelectionIndex ? projectableObjectsIndexed.Count - 1 : InitialSelectionIndex;
        InitialSelectionIndex = projectableObjectsIndexed.Count == 0 ? 0 : InitialSelectionIndex;
    }

    private void HandleProjectableObjectSelection()
    {
        if (projectableObjectsIndexed.Count <= 1) return;
        if (playerInteract.IsInteracting) return;
        if (playerInteractAlternate.IsInteractingAlternate) return;

        //HandleProjectableObjectSelectionNext();
        //HandleProjectableObjectSelectionPrevious();

        HandleProjectableObjectExactSlotSelection();
    }

    private void HandleProjectableObjectSelectionNext()
    {
        if (!SelectionInputNext) return;

        int previousIndex = currentSelectionIndex;
        int maxIndex = projectableObjectsIndexed.Count - 1;

        int desiredIndex = currentSelectionIndex + 1;

        currentSelectionIndex = desiredIndex > maxIndex ? 0 : desiredIndex;

        SelectProjectableObject(projectableObjectsIndexed[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this,new OnSelectionEventArgs { projectableObjectIndexed =  projectableObjectsIndexed[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this,new OnSelectionEventArgs { projectableObjectIndexed = selectedProjectableObjectIndexed });
    }

    private void HandleProjectableObjectSelectionPrevious()
    {
        if (!SelectionInputPrevious) return;

        int previousIndex = currentSelectionIndex;
        int maxIndex = projectableObjectsIndexed.Count - 1;

        int desiredIndex = currentSelectionIndex - 1;

        currentSelectionIndex = desiredIndex < 0 ? maxIndex : desiredIndex;

        SelectProjectableObject(projectableObjectsIndexed[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this, new OnSelectionEventArgs { projectableObjectIndexed = projectableObjectsIndexed[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { projectableObjectIndexed = selectedProjectableObjectIndexed });
    }

    private void HandleProjectableObjectExactSlotSelection()
    {
        HandleExactProjectableObjectSelection(_1stProjectableObjectInput, 0);
        HandleExactProjectableObjectSelection(_2ndProjectableObjectInput, 1);
        HandleExactProjectableObjectSelection(_3rdProjectableObjectInput, 2);
        HandleExactProjectableObjectSelection(_4thProjectableObjectInput, 3);
    }

    private void HandleExactProjectableObjectSelection(bool input, int index)
    {
        if (!input) return;
        if (currentSelectionIndex == index) return;
        if (projectableObjectsIndexed.Count <= index) return;

        int previousIndex = currentSelectionIndex;
        currentSelectionIndex = index;

        SelectProjectableObject(projectableObjectsIndexed[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this, new OnSelectionEventArgs { projectableObjectIndexed = projectableObjectsIndexed[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { projectableObjectIndexed = projectableObjectsIndexed[previousIndex] });
    }

    private void SelectProjectableObject(ProjectableObjectIndexed projectableObjectIndexed)
    {
        selectedProjectableObjectIndexed = projectableObjectIndexed; 
    }

    #region ObjectLearned
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e)
    {
        AddProjectableObjectToIndexedList(e.projectableObjectLearned);
    }

    

    #endregion

}
