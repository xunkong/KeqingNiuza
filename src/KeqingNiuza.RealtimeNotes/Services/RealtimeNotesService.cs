using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGP.Genshin.Common.Request;
using DGP.Genshin.Common.Request.DynamicSecret;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;
using DGP.Genshin.MiHoYoAPI.User;
using KeqingNiuza.RealtimeNotes.Models;

namespace KeqingNiuza.RealtimeNotes.Services
{
    internal class RealtimeNotesService
    {
        public static async Task<List<RealtimeNotesInfo>> GetRealtimeNotes(string cookieStr)
        {
            if (string.IsNullOrWhiteSpace(cookieStr))
            {
                return null;
            }
            var cookies = cookieStr.Split('#');
            var result = new List<RealtimeNotesInfo>();
            foreach (var cookie in cookies)
            {
                var roleClient = new UserGameRoleProvider(cookie);
                var roles = await roleClient.GetUserGameRolesAsync();
                foreach (var role in roles.List)
                {
                    var noteClient = new DailyNoteProvider(cookie);
                    var note = await noteClient.GetDailyNoteAsync(role.Region, role.GameUid);
                    var info = RealtimeNotesInfo.FromGameRole(role);
                    info.UpdateNoteInfo(note);
                    result.Add(info);
                }
            }
            return result;
        }
    }
}
