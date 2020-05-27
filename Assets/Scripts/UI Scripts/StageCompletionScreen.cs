using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

//Displayed at the end of a stage
public class StageCompletionScreen : MonoBehaviour
{
    public string nextStageName = "";
    public Text timeText;
    public Text enemiesKilledText;
    public Text damageTakenText;
    public Text damageDealtText;

    private void OnEnable()
    {
        SceneGoalManager goalManager = SceneGoalManager.goalManager;
        timeText.text = "Time spent: " + goalManager.StageDuration.ToString("N3") + "s";
        int enemiesKilled = goalManager.stageEnemyCountGoals.Sum();
        enemiesKilledText.text = "Enemies killed: " + enemiesKilled;
        damageTakenText.text = "Damage taken: " + Player.playerObject.GetComponent<Player>().damageReceived;
        damageDealtText.text = "Damage dealt: " + Player.playerObject.GetComponent<Player>().damageDealt;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(nextStageName);
        }
    }
}
