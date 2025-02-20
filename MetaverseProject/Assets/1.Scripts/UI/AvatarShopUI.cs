using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class AvatarShopUI : BaseUI
{
    protected override UIState GetUIState() => UIState.AvatarShop;

    [SerializeField] Sprite wizardSprite;
    [SerializeField] Sprite goblinSprite;

    Button wizardButton;
    Button goblinButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        wizardButton = transform.Find("WizardAvatarButton").GetComponent<Button>();
        goblinButton = transform.Find("GoblinAvatarButton").GetComponent<Button>();

        wizardButton.onClick.AddListener(SetAvatarWizardServerRpc);
        goblinButton.onClick.AddListener(SetAvatarGoblinServerRpc);
    }

    [ServerRpc]
    public void SetAvatarWizardServerRpc()
    {
        ulong userId = NetworkManager.Singleton.LocalClientId;
        NetworkObject userObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[userId];
        userObject.GetComponent<ResourceController>().ChangeAvatar(wizardSprite);
    }

    [ServerRpc]
    public void SetAvatarGoblinServerRpc()
    {
        ulong userId = NetworkManager.Singleton.LocalClientId;
        NetworkObject userObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[userId];
        userObject.GetComponent<ResourceController>().ChangeAvatar(goblinSprite);
    }
}
