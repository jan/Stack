using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class VersionUI : MonoBehaviour
{
    private void Start()
    {
        string version = "v" + Application.version;

        ApplicationManager.Branch branch = ApplicationManager.GetBranch();
        string addon = branch == ApplicationManager.Branch.Main ? "" : (" " + branch);

        GetComponent<TMP_Text>().text = version + addon;
    }
}
