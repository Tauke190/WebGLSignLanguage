using UnityEngine;

public class VisemeTest : MonoBehaviour
{
    public VisemeReceiver receiver; // assign your VisemeReceiver in inspector

    string testJson = @"[
        { ""visemeId"": 0, ""time"": 50 },
        { ""visemeId"": 13, ""time"": 100 },
        { ""visemeId"": 4, ""time"": 275 },
        { ""visemeId"": 6, ""time"": 375 },
        { ""visemeId"": 19, ""time"": 475 },
        { ""visemeId"": 0, ""time"": 550 },
        { ""visemeId"": 13, ""time"": 775 },
        { ""visemeId"": 4, ""time"": 838 },
        { ""visemeId"": 6, ""time"": 931 },
        { ""visemeId"": 19, ""time"": 1025 },
        { ""visemeId"": 0, ""time"": 1100 },
        { ""visemeId"": 13, ""time"": 1325 },
        { ""visemeId"": 4, ""time"": 1388 },
        { ""visemeId"": 6, ""time"": 1481 },
        { ""visemeId"": 19, ""time"": 1575 },
        { ""visemeId"": 0, ""time"": 1637 },
        { ""visemeId"": 13, ""time"": 1850 },
        { ""visemeId"": 4, ""time"": 1913 },
        { ""visemeId"": 6, ""time"": 2006 },
        { ""visemeId"": 19, ""time"": 2100 },
        { ""visemeId"": 0, ""time"": 2275 }
        ]";

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)) {
            receiver.ReceiveViseme(testJson);
        }
    }
}
