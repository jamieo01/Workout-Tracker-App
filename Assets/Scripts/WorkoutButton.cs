using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutButton : MonoBehaviour
{
    private void Start()
    {
        Text text = GetComponentInChildren<Text>();
        text.text = gameObject.name;
    }
    public void StartWorkout() 
    {
        ExerciseManager.Instance._currentWorkout = ExerciseManager.Instance.GetWorkout(this.gameObject.name);
        ExerciseManager.Instance.StartWorkout();
        Debug.Log(ExerciseManager.Instance._currentWorkout._workoutName);
    }


    public void ViewWorkout() 
    {
        ExerciseManager.Instance.ViewWorkoutDetails(gameObject.name);
    
    }

    public void DeleteWorkout() 
    {
        Workout workoutToDelete = ExerciseManager.Instance.GetWorkout(gameObject.name);
        ExerciseManager.Instance.DeleteWorkout(workoutToDelete);
       
        Destroy(this.gameObject);
    }
}
