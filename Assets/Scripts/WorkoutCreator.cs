using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkoutCreator : MonoBehaviour
{
    [SerializeField] private GameObject _addExerciseDetailsPrefab;
    [SerializeField] private GameObject _addExerciseButton;
    [SerializeField] private GameObject _exerciseParent;


    private List<GameObject> _currentExerciseObjects = new List<GameObject>();
    private List<Exercise> _exercises = new List<Exercise>();
    TMP_InputField _exerciseNameTextArea = null;
    TMP_InputField _exerciseRepsTextArea = null;
    TMP_InputField _exerciseSetsTextArea;
    TMP_InputField _exerciseLastSetsTextArea;
    TMP_InputField _exerciseLastRepsTextArea;
    TMP_InputField _workoutNameTextArea;
    Exercise _exercise;

    private void Start()
    {
        _currentExerciseObjects.Add(Instantiate(_addExerciseDetailsPrefab, _exerciseParent.transform));

        
        _workoutNameTextArea = transform.Find("Workout Name").GetComponent<TMP_InputField>();
    }
    public void AddExercises()
    {
        foreach (var item in _currentExerciseObjects)
        {
            _exerciseNameTextArea = item.transform.Find("Exercise Name").GetComponent<TMP_InputField>();
            _exerciseRepsTextArea = item.transform.Find("Exercise Reps").GetComponent<TMP_InputField>();
            _exerciseSetsTextArea = item.transform.Find("Exercise Sets").GetComponent<TMP_InputField>();
            _exerciseLastRepsTextArea = item.transform.Find("Exercise Last Reps").GetComponent<TMP_InputField>();
            _exerciseLastSetsTextArea = item.transform.Find("Exercise Last Sets").GetComponent<TMP_InputField>();

            _exercise.name = _exerciseNameTextArea.text;
            _exercise.reps = (int)Helper.CheckIfNumber(_exerciseRepsTextArea.text);
            _exercise.sets = (int)Helper.CheckIfNumber(_exerciseSetsTextArea.text);
            _exercise.lastReps = (int)Helper.CheckIfNumber(_exerciseLastRepsTextArea.text);
            _exercise.lastSets = (int)Helper.CheckIfNumber(_exerciseLastSetsTextArea.text);
            _exercises.Add(_exercise);
        }
        
        //_addExerciseButton.transform.position = new Vector3(_addExerciseButton.transform.position.x, _currentExerciseObjects.transform.position.y, _addExerciseButton.transform.position.z);
    }

    public void SaveWorkout() 
    {
        AddExercises();
        Workout newWorkout = new Workout( );
        newWorkout._workoutName = _workoutNameTextArea.text;
        newWorkout.exercisesInWorkout = _exercises;
        ExerciseManager.Instance.SaveWorkout(newWorkout);
    }
 



    public void AddNewButton() => _currentExerciseObjects.Add(Instantiate(_addExerciseDetailsPrefab, _exerciseParent.transform));
    
}
