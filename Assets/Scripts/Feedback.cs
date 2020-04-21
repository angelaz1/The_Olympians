using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feedback
{
    private int addedFollowers;
    private string feedbackText;
    private string feedbackName;

    public Feedback(int addedFollowers, string feedbackText) {
        this.addedFollowers = addedFollowers;
        this.feedbackText = feedbackText;
        this.feedbackName = "xoxoHermes240";
    }

    public string getFeedbackText() {
        return feedbackText;
    }

    public string getFeedbackName() {
        return feedbackName;
    }

    public int getAddedFollowers() {
        return addedFollowers;
    }
}
