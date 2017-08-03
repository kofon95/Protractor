using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputValidator : MonoBehaviour
{
    public string expected;
    public string id;
    Text textUI;
    static Dictionary<string, Text> publicValues = new Dictionary<string, Text>();
    private bool isValid;

    public static int ValidTextUICount { get; private set; }
    public static int TextUICount { get; private set; }

    void Start()
    {
        textUI = GetComponent<InputField>().textComponent;
        if (!string.IsNullOrEmpty(id))
            publicValues[id] = textUI;

        isValid = textUI.text == expected;
        if (isValid)
            ValidTextUICount++;
        TextUICount++;
    }

    public void OnEditChanged()
    {
        var isValidForNow = textUI.text == expected;
        if (isValidForNow == isValid) return; // no changes

        ValidTextUICount += isValidForNow ? 1 : -1;
        isValid = isValidForNow;

        print(ValidTextUICount);
        print(AllTextAreValid);
        if (ValidTextUICount == TextUICount && AllTextAreValid != null)
            AllTextAreValid(ValidTextUICount);
    }

    public static Text GetTextUIById(string id)
    {
        Text value;
        publicValues.TryGetValue(id, out value);
        return value;
    }

    public static event System.Action<int> AllTextAreValid;
}
