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
    [SerializeField] private ProjectableObjectSO selectedProjectableObjectSO;

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

    public List<ProjectableObjectSO> ProjectableObjectsInventory => ProjectableObjectsLearningManager.Instance.ProjectableObjectsLearned;
    public ProjectableObjectSO SelectedProjectableObjectSO => selectedProjectableObjectSO;
    public int CurrentSelectionIndex => currentSelectionIndex;

    public class OnObjectAddedToInventoryEventArgs : EventArgs
    {
        public ProjectableObjectSO projectableObjectSO;
    }

    public class OnSelectionEventArgs : EventArgs
    {
        public int index;
        public ProjectableObjectSO projectableObjectSO;
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
        ClampInitialSelectionIndex();

        if (ProjectableObjectsInventory.Count > 0) SelectProjectableObject(ProjectableObjectsInventory[InitialSelectionIndex]);

        currentSelectionIndex = InitialSelectionIndex;

        OnProjectableObjectSelectionManagerInitialized?.Invoke(this, EventArgs.Empty);
    }

    private void ClampInitialSelectionIndex()
    {
        InitialSelectionIndex = ProjectableObjectsInventory.Count <= InitialSelectionIndex ? ProjectableObjectsInventory.Count - 1 : InitialSelectionIndex;
        InitialSelectionIndex = ProjectableObjectsInventory.Count == 0 ? 0 : InitialSelectionIndex;
    }

    private void HandleProjectableObjectSelection()
    {
        if (ProjectableObjectsInventory.Count <= 1) return;
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
        int maxIndex = ProjectableObjectsInventory.Count - 1;

        int desiredIndex = currentSelectionIndex + 1;

        currentSelectionIndex = desiredIndex > maxIndex ? 0 : desiredIndex;

        SelectProjectableObject(ProjectableObjectsInventory[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this,new OnSelectionEventArgs { index = previousIndex, projectableObjectSO = ProjectableObjectsInventory[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this,new OnSelectionEventArgs { index = currentSelectionIndex, projectableObjectSO = ProjectableObjectsInventory[currentSelectionIndex] });
    }

    private void HandleProjectableObjectSelectionPrevious()
    {
        if (!SelectionInputPrevious) return;

        int previousIndex = currentSelectionIndex;
        int maxIndex = ProjectableObjectsInventory.Count - 1;

        int desiredIndex = currentSelectionIndex - 1;

        currentSelectionIndex = desiredIndex < 0 ? maxIndex : desiredIndex;

        SelectProjectableObject(ProjectableObjectsInventory[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this, new OnSelectionEventArgs { index = previousIndex, projectableObjectSO = ProjectableObjectsInventory[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { index = currentSelectionIndex, projectableObjectSO = ProjectableObjectsInventory[currentSelectionIndex] });
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
        if (ProjectableObjectsInventory.Count <= index) return;

        int previousIndex = currentSelectionIndex;
        currentSelectionIndex = index;

        SelectProjectableObject(ProjectableObjectsInventory[currentSelectionIndex]);

        OnProjectableObjectDeselected?.Invoke(this, new OnSelectionEventArgs { index = previousIndex, projectableObjectSO = ProjectableObjectsInventory[previousIndex] });
        OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { index = currentSelectionIndex, projectableObjectSO = ProjectableObjectsInventory[currentSelectionIndex] });
    }

    private void SelectProjectableObject(ProjectableObjectSO projectableObjectSO)
    {
        if (!projectableObjectSO) return;
        selectedProjectableObjectSO = projectableObjectSO; 
    }

    #region ObjectLearned
    private void ProjectableObjectsLearningManager_OnProjectableObjectLearned(object sender, ProjectableObjectsLearningManager.OnProjectableObjectLearnedEventArgs e)
    {
        OnObjectAddedToInventory?.Invoke(this, new OnObjectAddedToInventoryEventArgs { projectableObjectSO = e.projectableObjectLearned });

        if (ProjectableObjectsInventory.Count <= 1)
        {
            currentSelectionIndex = 0;
            SelectProjectableObject(ProjectableObjectsInventory[currentSelectionIndex]);
            OnProjectableObjectSelected?.Invoke(this, new OnSelectionEventArgs { index = currentSelectionIndex, projectableObjectSO = ProjectableObjectsInventory[currentSelectionIndex] });
        }
    }

    #endregion

}
