using UnityEngine;
using UnityEngine.InputSystem; // <-- replaces old Input
using TMPro;
using System.Collections.Generic;

public class TutorialSequence : MonoBehaviour
{
    
    private float lastAdvanceTime = -999f;
    public float advanceCooldown = 0.2f;
    [System.Serializable]
    public class TutorialStep
    {
        public TMP_Text text;
        public bool isConditional;
        public string conditionKey;
    }

    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private TutorialStep[] steps;
    private int currentIndex = 0;
    private HashSet<string> metConditions = new HashSet<string>();

    void Start()
    {
        foreach (var step in steps)
            step.text.gameObject.SetActive(false);

        ShowStep(0);
    }

    void Update()
    {
        
    }

    //when called, proceeds to the next step
    public void Advance()
    {
        
        if (Time.time - lastAdvanceTime < advanceCooldown) return;
        lastAdvanceTime = Time.time;

        // If current step is conditional and locked, block
        if (currentIndex < steps.Length && steps[currentIndex].isConditional
            && !metConditions.Contains(steps[currentIndex].conditionKey))
        {
            
            return;
        }

        // Hide current
        if (currentIndex < steps.Length)
            steps[currentIndex].text.gameObject.SetActive(false);

        int next = currentIndex + 1;

        // Don't show next step if it's conditional and locked
        if (next < steps.Length && steps[next].isConditional
            && !metConditions.Contains(steps[next].conditionKey))
        {
            
            currentIndex = next; // park here so UnlockCondition can find it
            UpdatePanelVisibility();
            return; // don't call ShowStep
        }

        currentIndex = next;
        ShowStep(currentIndex);
        UpdatePanelVisibility();
    }
    //sets the next step text to be visible
    void ShowStep(int index)
    {
        if (index < steps.Length)
            steps[index].text.gameObject.SetActive(true);

        UpdatePanelVisibility();
    }

    //called from wherever the unlock should occur
    //when the player presses a button, unlocks the text so it can appear
    public void UnlockCondition(string key) 
    {
       //key meets conditions
        metConditions.Add(key);


        if (currentIndex < steps.Length)
        {
            var current = steps[currentIndex];
            //if the current index is conditional amd the conditional key for that is unlocked
            if (current.isConditional && current.conditionKey == key)
            {
                //calls this function
                ShowStep(currentIndex);
            }
        }
        UpdatePanelVisibility();
    }

    //Makes sure that the visibility of the panel is only active when there should be text appearing
    private void UpdatePanelVisibility()
    {
        bool textActive = false;
        foreach(var step in steps)
        {
            if (step.text.gameObject.activeSelf)
            {
                textActive = true;
                break;
            }
        }
        tutorialPanel.SetActive(textActive);
    }
}