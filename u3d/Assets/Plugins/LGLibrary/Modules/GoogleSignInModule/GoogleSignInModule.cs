using System.Threading.Tasks;
using Google;
using UnityEngine;

namespace LG
{
  public class GoogleSignInModule : ModuleBase<GoogleSignInModule>
  {
    private string WebClientId;
    public override void LGLoad(params object[] agrs)
    {
      if (agrs.Length == 1 && agrs[0] is string)
      {
        WebClientId = agrs[0] as string;
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
          RequestIdToken = true,
          WebClientId = WebClientId,
        };
        base.LGLoad(agrs);
      }
    }

    //登录
    public Task<GoogleSignInUser> SignIn()
    {
      Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
      return signIn;
    }
  }
}