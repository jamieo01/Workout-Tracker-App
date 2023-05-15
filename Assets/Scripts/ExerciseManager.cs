using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ExerciseManager : MonoBehaviour
{
    public static ExerciseManager Instance;
    private void Awake() => Instance = this;

    private Exercise _currentExercise;
    public Workout _currentWorkout;
    private int _currentSet = 1;
    private int _exerciseIndex = 0;

    [SerializeField] private GameObject _currentExerciseInfo;

    #region Current Exercise Info
    [SerializeField] private TextMeshProUGUI _exerciseTitle;
    [SerializeField] private TextMeshProUGUI _exerciseSet;
    [SerializeField] private TextMeshProUGUI _exerciseReps;
    [SerializeField] private TextMeshProUGUI _lastReps;
    [SerializeField] private TextMeshProUGUI _lastSets;
    [SerializeField] private TextMeshProUGUI _lastWeight;
    [SerializeField] private TMP_InputField _weightInfo;
    [SerializeField] private TMP_InputField _repInfo;

    [SerializeField] private TextMeshProUGUI _nextExerciseText;
    #endregion

    [SerializeField] private GameObject _workoutDetailsMenu;

    [SerializeField] string[] strings = { "Hello", "Fuck" };
    [SerializeField] private Canvas _menuCanvas;

    [SerializeField] public List<Workout> _workouts = new List<Workout>();
    private List<Exercise> _exercises = new List<Exercise>();
    [SerializeField] private GameObject _workoutButtonPrefab;
    [SerializeField] private Transform _workoutParent;
    private bool _timerOnlyMode;
    [SerializeField] private SetTimer _setTimer;
    bool _startedWorkout = false;

    public static bool HasEnded; 
 
    private int _lastWorkoutTotals = 0;

    public bool IsTimerOnlyMode => _timerOnlyMode;

    public Workout GetWorkout(string workoutToFind)
    {
        Workout workout = new Workout();

        foreach (var item in _workouts)
        {
            if (item._workoutName == workoutToFind) workout = item;
        }
        return workout;
    }

    public void DeleteWorkout(Workout workoutStringToDelete) 
    {
        _workouts.Remove(workoutStringToDelete);
        UpdateWorkouts();

    }

   public void UpdateWorkouts() 
    {
        if (_timerOnlyMode) return;
        foreach (var item in _workouts.ToArray())
        {
            SaveWorkout(item);
        }
        _lastWorkoutTotals = _workouts.Count;
    }
    void Start()
    {
        _currentWorkout._workoutName = string.Empty;
        if (!PlayerPrefs.HasKey("Workouts")) {
            var defaultWorkoutString = Resources.Load<TextAsset>("DefaultWorkout");

            var tempWorkoutCollection = JsonUtility.FromJson<WorkoutCollection>(defaultWorkoutString.ToString());
 
            foreach (var item in tempWorkoutCollection.workouts)
            {
                Debug.Log("Saving Default Workouts");
                SaveWorkout(item);
            }
        }
   
       _workouts = LoadWorkouts().workouts;

        foreach (var item in _workouts)
        {
            GameObject tempButton = Instantiate(_workoutButtonPrefab, _workoutParent);
            tempButton.name = item._workoutName;
        }
        SetTimer.OnTimerEnd += IncramentRep;
        SetTimer.OnTimerEnd += UpdateExercise;
        RestTimer.OnRestStart += UpdateExercise;
        
        
        
        _lastWorkoutTotals = _workouts.Count;

    }


    void Update()
    {

      

        if (_lastWorkoutTotals != _workouts.Count && _currentSet>1) UpdateWorkouts();

        if (_currentExercise != null && _currentSet== _currentExercise.sets  && !HasEnded )
        {
            if (_exerciseIndex < _currentWorkout.exercisesInWorkout.Count - 1) _currentExercise = _currentWorkout.exercisesInWorkout[++_exerciseIndex];
            else if (_currentSet == _currentExercise.sets) HasEnded = true;
            _currentSet = 0;
        }
        
    }

    Exercise SetUpWorkout(string name, int reps, int sets)
    {
        Exercise exercise;
        exercise = new Exercise(name, reps, sets, 0, 0, 0);
        return exercise;

    }

    private void IncramentRep() => _currentSet++;


    public void StartWorkout()
    {
        if (_timerOnlyMode) return;
        _currentExercise = _currentWorkout.exercisesInWorkout[0];

        UpdateExercise();
        MenuStateSwitch();
        _startedWorkout = true;
    }

    public void EndWorkout()
    {
       
        if (_timerOnlyMode) return;
        SaveWorkout(_currentWorkout);
        _startedWorkout = false;
       
        MenuStateSwitch();
    }

    public void SaveWorkout(Workout workoutToSave)
    {
        string workoutString;
        if (!_workouts.Contains(workoutToSave)) { _workouts.Add(workoutToSave);  Debug.Log("Contains"); }
        WorkoutCollection workoutCollection = new WorkoutCollection();
        workoutCollection.workouts = _workouts;
        workoutString = JsonUtility.ToJson(workoutCollection, true);
        
        PlayerPrefs.SetString("Workouts", workoutString);

    }
    void AddExercise()
    {



    }

    WorkoutCollection LoadWorkouts()
    {

        return JsonUtility.FromJson<WorkoutCollection>(PlayerPrefs.GetString("Workouts"));
    }
    public void StartTimerOnlyMode()
    {
        _timerOnlyMode = true;
        _currentExerciseInfo.SetActive(false);
        MenuStateSwitch();

    }

    public void ViewWorkoutDetails(string workoutName)
    {
        
        Workout workoutToView = GetWorkout(workoutName);
        _workoutDetailsMenu.SetActive(true);
        Debug.Log(this.gameObject);
        _workoutDetailsMenu.transform.Find("Workout Name").GetComponent<TextMeshProUGUI>().SetText(workoutToView._workoutName); ;
        TextMeshProUGUI tempWorkoutDetails= _workoutDetailsMenu.transform.Find("Workout Details").GetComponent<TextMeshProUGUI>();
        tempWorkoutDetails.text = "";
        foreach (var item in workoutToView.exercisesInWorkout)
        {
            tempWorkoutDetails.text += $"  \n {item.name}\n Last Reps: {item.lastReps} Last Sets: {item.lastSets}  Last Weight: {item.lastWeight}";
        }
    }

    private void UpdateExercise()
    {
        if (_timerOnlyMode) return;
        if (_currentSet > 1)
        {
           if(Helper.CheckIfNumber(_weightInfo.text) > 0)  _currentExercise.lastWeight = Helper.CheckIfNumber(_weightInfo.text); 
            if(Helper.CheckIfNumber(_repInfo.text) > 0) _currentExercise.lastReps = (int)Helper.CheckIfNumber(_repInfo.text); 
        }

        _exerciseTitle.SetText(_currentExercise.name);
        _exerciseSet.SetText($"Set:{_currentSet} / {_currentExercise.sets}");
        _exerciseReps.SetText($"Target Reps: {_currentExercise.reps.ToString()}");
        _lastReps.SetText($"Last Reps: {_currentExercise.lastReps.ToString()}");
        _lastSets.SetText($"Last Sets: {_currentExercise.lastSets.ToString()}");
        _lastWeight.SetText($"Last Weight: {_currentExercise.lastWeight}");

        if (_currentWorkout.exercisesInWorkout.Count > _exerciseIndex + 1) _nextExerciseText.SetText($"{_currentWorkout.exercisesInWorkout[_exerciseIndex + 1].name}");
        else _nextExerciseText.SetText("");
    }
    public void ReturnToMenu()
    {
        MenuStateSwitch();
    }

    private void MenuStateSwitch()
    {
        _menuCanvas.enabled = !_menuCanvas.enabled;
    }

    public void ClearWorkouts()
    {
        PlayerPrefs.DeleteKey("Workouts");

    }
}
