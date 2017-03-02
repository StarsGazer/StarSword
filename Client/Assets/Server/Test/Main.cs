using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 网络测试
/// </summary>
public class Main : UnitySingleton<Main>
{
    public Button loginBtn;
    public Button registerBtn;
    public Button loginCloseBtn;
    public Button registerCloseBtn;
    public Button confirmBtn_Login;
    public Button confirmBtn_Register;
    public Button cancelBtn_Login;
    public Button cancelBtn_Register;
    public InputField usernameInput_Login;
    public InputField passwordInput_Login;
    public InputField usernameInput_Register;
    public InputField passwordInput_Register;
    public InputField password2Input_Register;
    public GameObject loginPanel;
    public GameObject registerPanel;

    public Button select1;
    public Button select2;
    public Button select3;
    public Button select4;
    public InputField nickNameInput;
    public GameObject createRolePanel;

    public override void Awake()
    {
        loginBtn.onClick.AddListener(delegate { OnLoginBtnClick(); });
        registerBtn.onClick.AddListener(delegate { OnRegisterBtnClick(); });
        loginCloseBtn.onClick.AddListener(delegate { OnLoginCloseBtnClick(); });
        registerCloseBtn.onClick.AddListener(delegate { OnRegisterCloseBtnClick(); });
        confirmBtn_Login.onClick.AddListener(delegate { OnLoginConfirmBtnClick(); });
        confirmBtn_Register.onClick.AddListener(delegate { OnRegisterConfirmBtnClick(); });
        cancelBtn_Login.onClick.AddListener(delegate { OnLoginCancelBtnClick(); });
        cancelBtn_Register.onClick.AddListener(delegate { OnRegisterCancelBtnClick(); });
        select1.onClick.AddListener(delegate { OnSelect1BtnClick(); });
        select2.onClick.AddListener(delegate { OnSelect2BtnClick(); });
        select3.onClick.AddListener(delegate { OnSelect3BtnClick(); });
        select4.onClick.AddListener(delegate { OnSelect4BtnClick(); });
    }

    void OnLoginBtnClick()
    {
        loginPanel.SetActive(true);
    }

    void OnRegisterBtnClick()
    {
        registerPanel.SetActive(true);
    }

    void OnLoginCloseBtnClick()
    {
        ClearInfo();
        loginPanel.SetActive(false);
    }

    void OnRegisterCloseBtnClick()
    {
        ClearInfo();
        registerPanel.SetActive(false);
    }

    void OnLoginConfirmBtnClick()
    {
        string username = usernameInput_Login.text;
        string password = passwordInput_Login.text;
        if (username.Length < 3 || username.Length > 10
            || password.Length < 3 || password.Length > 10)
        {
            Debug.Log("账号密码不合法!!!");
        }
        else
        {
            // ----------登录请求----------
            SocketManager.Instance.SendMsg_Login(username, password);

            // ----------网络测试----------
            /*
            for (int i = 0; i < 1; ++i)
            {
                StartCoroutine(Test(i));
            }
            */
        }
        ClearInfo();
    }

    IEnumerator Test(int i)
    {
        string username = i + "";
        string password = i + "";
        SocketManager.Instance.SendMsg_Login(username, password);
        yield return new WaitForSeconds(.005f);
    }

    void OnRegisterConfirmBtnClick()
    {
        string username = usernameInput_Register.text;
        string password = passwordInput_Register.text;
        string password2 = password2Input_Register.text;
        if (username.Length < 3 || username.Length > 10
            || password.Length < 3 || password.Length > 10
            || password2.Length < 3 || password2.Length > 10
            || !password.Equals(password2) )
        {
            Debug.Log("账号密码不合法!!!");
        }
        else
        {
            // ----------注册请求----------
            SocketManager.Instance.SendMsg_Register(username, password);
        }
        ClearInfo();
    }

    void OnLoginCancelBtnClick()
    {
        ClearInfo();
        loginPanel.SetActive(false);
    }

    void OnRegisterCancelBtnClick()
    {
        ClearInfo();
        registerPanel.SetActive(false);
    }

    void ClearInfo()
    {
        usernameInput_Login.text = "";
        passwordInput_Login.text = "";
        usernameInput_Register.text = "";
        passwordInput_Register.text = "";
        password2Input_Register.text = "";
    }

    ////////////////////
    string nickName = "";  // 昵称
    int profession = 1;  // 职业
    bool sex = true;  // 性别

    void OnSelect1BtnClick()
    {
        nickName = nickNameInput.text;
        if (nickName.Length < 3 || nickName.Length > 10)
            return;
        profession = 1;
        sex = true;
        SocketManager.Instance.SendMsg_CreateRole(nickName, profession, sex);
    }
    void OnSelect2BtnClick()
    {
        nickName = nickNameInput.text;
        if (nickName.Length < 3 || nickName.Length > 10)
            return;
        profession = 2;
        sex = false;
        SocketManager.Instance.SendMsg_CreateRole(nickName, profession, sex);
    }
    void OnSelect3BtnClick()
    {
        nickName = nickNameInput.text;
        if (nickName.Length < 3 || nickName.Length > 10)
            return;
        profession = 3;
        sex = true;
        SocketManager.Instance.SendMsg_CreateRole(nickName, profession, sex);
    }
    void OnSelect4BtnClick()
    {
        nickName = nickNameInput.text;
        if (nickName.Length < 3 || nickName.Length > 10)
            return;
        profession = 4;
        sex = false;
        SocketManager.Instance.SendMsg_CreateRole(nickName, profession, sex);
    }
    ////////////////////

    public void ShowCreateRolePanel()
    {
        createRolePanel.SetActive(true);
    }

    public void EnterScene()
    {
        SocketManager.Instance.SendMsg_LoginRole(Globals.Instance.PlayerManager.characterId);
    }
}
