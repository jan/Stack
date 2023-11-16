using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class VersionUI : MonoBehaviour
{
    private void Start()
    {
        string version = "v" + Application.version;

        ApplicationController.Branch branch = ApplicationController.GetBranch();
        string addon = branch == ApplicationController.Branch.Main ? "" : (" " + branch);

        GetComponent<TMP_Text>().text = version + addon;
    }
}
