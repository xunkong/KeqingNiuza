using System;
using System.Threading.Tasks;

namespace KeqingNiuza.Core.DailyCheck
{
    public class Program
    {

        public static Action<string> PrintLog { get; set; }

        public static async Task Checkin(string[] args)
        {
            PrintLog("开始签到");

            if (args.Length <= 0)
            {
                throw new InvalidOperationException("获取参数不对");
            }

            try
            {


                var cookieString = string.Join(" ", args);

                var cookies = cookieString.Split('#');

                int accountNum = 0;

                foreach (var cookie in cookies)
                {
                    accountNum++;

                    PrintLog($"开始签到 账号{accountNum}");

                    var client = new GenShinClient(
                        cookie);

                    var rolesResult =
                        await client.GetExecuteRequest<UserGameRolesEntity>(Config.GetUserGameRolesByCookie,
                            "game_biz=hk4e_cn");

                    //检查第一步获取账号信息
                    rolesResult.CheckOutCodeAndSleep();

                    int accountBindCount = rolesResult.Data.List.Count;

                    PrintLog($"账号{accountNum}绑定了{accountBindCount}个角色");

                    for (int i = 0; i < accountBindCount; i++)
                    {
                        PrintLog(rolesResult.Data.List[i].ToString());

                        var roles = rolesResult.Data.List[i];

                        var signDayResult = await client.GetExecuteRequest<SignDayEntity>(Config.GetBbsSignRewardInfo,
                            $"act_id={Config.ActId}&region={roles.Region}&uid={roles.GameUid}");

                        //检查第二步是否签到
                        signDayResult.CheckOutCodeAndSleep();

                        PrintLog(signDayResult.Data.ToString());

                        var data = new
                        {
                            act_id = Config.ActId,
                            region = roles.Region,
                            uid = roles.GameUid
                        };

                        var signClient = new GenShinClient(cookie, true);

                        var result =
                            await signClient.PostExecuteRequest<SignResultEntity>(Config.PostSignInfo,
                                jsonContent: new JsonContent(data));

                        PrintLog(result.CheckOutCodeAndSleep());
                    }
                }
            }
            catch (GenShinException e)
            {
                PrintLog($"请求接口时出现异常：{e.Message}");
                throw;
            }
            catch (System.Exception e)
            {
                PrintLog($"出现意料以外的异常：{e}");
                throw;
            }
            //抛出异常主动构建失败
            PrintLog("签到结束");
        }
    }
}
