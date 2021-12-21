using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeqingNiuza.RealtimeNotes.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using DGP.Genshin.MiHoYoAPI.Record.DailyNote;

namespace KeqingNiuza.RealtimeNotes.Services
{
    internal class TileService
    {
        public static async Task<bool> RequestPinTileAsync(RealtimeNotesInfo info)
        {
            SecondaryTile tile = new SecondaryTile(info.Uid,
                                                   "实时便笺",
                                                   "--update-note",
                                                   new Uri("ms-appx:///Assets/Square150x150Logo.png"),
                                                   Windows.UI.StartScreen.TileSize.Wide310x150);
            tile.VisualElements.Square44x44Logo = new Uri("ms-appx:///Assets/Square44x44Logo.png");
            tile.VisualElements.Square71x71Logo = new Uri("ms-appx:///Assets/Square71x71Logo.png");
            tile.VisualElements.Square150x150Logo = new Uri("ms-appx:///Assets/Square150x150Logo.png");
            tile.VisualElements.Wide310x150Logo = new Uri("ms-appx:///Assets/Square310x150Logo.png");
            tile.VisualElements.Square310x310Logo = new Uri("ms-appx:///Assets/Square310x310Logo.png");
            // Assign the window handle
            IInitializeWithWindow initWindow = (IInitializeWithWindow)(object)tile;
            initWindow.Initialize(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle);
            // Pin the tile
            bool isPinned = await tile.RequestCreateAsync();

            // TODO: Update UI to reflect whether user can now either unpin or pin
            return isPinned;
        }




        public static async Task<List<string>> FindAllAsync()
        {
            var tiles = await SecondaryTile.FindAllAsync();
            return tiles.Select(x => x.TileId).ToList();
        }


        public static void UpdateTile(RealtimeNotesInfo info)
        {
            if (info.Expeditions.Count < 5)
            {
                do
                {
                    info.Expeditions.Add(new Expedition { AvatarSideIcon = Path.Combine(AppContext.BaseDirectory, "resource/others/Transparent.png") });
                } while (info.Expeditions.Count != 5);
            }
            var updater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(info.Uid);
            var content = GetTileContent(info);
            var notification = new TileNotification(content.GetXml());
            updater.Update(notification);
        }



        /// <summary>
        /// 磁贴样式
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static TileContent GetTileContent(RealtimeNotesInfo info)
        {
            return new TileContent
            {
                Visual = new TileVisual
                {
                    BaseUri = new Uri(AppContext.BaseDirectory),
                    // 小磁贴
                    TileSmall = new TileBinding
                    {
                        Content = new TileBindingContentAdaptive
                        {
                            Children =
                            {
                                new AdaptiveImage
                                {
                                    Source = "resource/others/Resin_24.png",
                                    HintAlign = AdaptiveImageAlign.Center,
                                    HintRemoveMargin = true,
                                },
                                new AdaptiveText
                                {
                                    Text = info.CurrentResin.ToString(),
                                    HintAlign = AdaptiveTextAlign.Center,
                                }
                            }
                        }
                    },
                    // 中磁贴
                    TileMedium = new TileBinding
                    {
                        Content = new TileBindingContentAdaptive
                        {
                            Children =
                            {
                                // 个人信息
                                new AdaptiveText
                                {
                                    Text = info.NickName,
                                    HintAlign =AdaptiveTextAlign.Left
                                },
                                new AdaptiveText
                                {
                                    Text = $"更新于 {DateTime.Now:HH:mm}",
                                    HintAlign =AdaptiveTextAlign.Left
                                },
                                new AdaptiveText(),
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        // 树脂
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Resin.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.CurrentResin.ToString(),
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        // 树脂恢复时间
                                        //new AdaptiveSubgroup
                                        //{
                                        //    HintWeight = 2,
                                        //    Children =
                                        //    {
                                        //        new AdaptiveImage
                                        //        {
                                        //            Source = "resource/others/Resin2.png",
                                        //            HintAlign = AdaptiveImageAlign.Stretch,
                                        //            HintRemoveMargin = true,
                                        //        },
                                        //        new AdaptiveText
                                        //        {
                                        //            Text = info.LastUpdateTime.AddSeconds(info.ResinRecoveryTime).ToString("HH:mm"),
                                        //            HintAlign = AdaptiveTextAlign.Center,
                                        //        }
                                        //    }
                                        //},
                                        // 委托
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Commission.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.IsExtraTaskRewardReceived? "√" : $"{info.FinishedTaskNum}/{info.TotalTaskNum}",
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        // 周本
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Domain.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = $"{info.RemainResinDiscountNum}/{info.ResinDiscountNumLimit}",
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    },
                    // 宽磁贴
                    TileWide = new TileBinding
                    {
                        Content = new TileBindingContentAdaptive
                        {
                            Children =
                            {
                                // 个人信息
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup
                                        {
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text = info.NickName,
                                                    HintAlign= AdaptiveTextAlign.Left,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text=$"更新于 {info.LastUpdateTime:HH:mm}",
                                                     HintAlign= AdaptiveTextAlign.Right,
                                                }
                                            }
                                        }
                                    }
                                },
                                // 第二行
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        // 树脂
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Resin.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text =$"{info.CurrentResin}",
                                                    HintAlign = AdaptiveTextAlign.Left,
                                                },
                                            }
                                        },
                                        // 树脂恢复时间
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight= 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Resin2.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight= 3,
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text = info.LastUpdateTime.AddSeconds(info.ResinRecoveryTime).ToString("HH:mm"),
                                                    HintAlign = AdaptiveTextAlign.Left,
                                                },
                                            }
                                        },
                                        // 委托
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight= 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Commission.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight= 2,
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text = info.IsExtraTaskRewardReceived? "√" : $"{info.FinishedTaskNum}/{info.TotalTaskNum}",
                                                    HintAlign = AdaptiveTextAlign.Left,
                                                },
                                            }
                                        },
                                        // 周本
                                        //new AdaptiveSubgroup
                                        //{
                                        //    HintWeight = 2,
                                        //    Children =
                                        //    {
                                        //        new AdaptiveImage
                                        //        {
                                        //            Source = "resource/others/Domain.png",
                                        //            HintAlign = AdaptiveImageAlign.Stretch,
                                        //            HintRemoveMargin = true,
                                        //        },
                                        //    }
                                        //},
                                        //new AdaptiveSubgroup
                                        //{
                                        //    HintWeight = 2,
                                        //    Children =
                                        //    {
                                        //        new AdaptiveText
                                        //        {
                                        //            Text = $"{info.RemainResinDiscountNum}/{info.ResinDiscountNumLimit}",
                                        //            HintAlign = AdaptiveTextAlign.Left,
                                        //        }
                                        //    }
                                        //},
                                    }
                                },
                                // 派遣
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[0].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[0].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[1].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[1].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[2].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[2].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[3].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[3].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[4].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[4].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    },
                    // 大磁贴
                    TileLarge = new TileBinding
                    {
                        Content = new TileBindingContentAdaptive
                        {
                            Children =
                            {
                                // 个人信息
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup
                                        {
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text = info.NickName,
                                                    HintAlign= AdaptiveTextAlign.Left,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            Children =
                                            {
                                                new AdaptiveText
                                                {
                                                    Text=$"更新于 {info.LastUpdateTime:HH:mm}",
                                                     HintAlign= AdaptiveTextAlign.Right,
                                                }
                                            }
                                        }
                                    }
                                },
                                new AdaptiveText(),
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        // 树脂
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Resin.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = false,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text =$"{info.CurrentResin}",
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                },
                                            }
                                        },
                                        // 树脂恢复时间
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Resin2.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = false,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.LastUpdateTime.AddSeconds(info.ResinRecoveryTime).ToString("HH:mm"),
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                },
                                            }
                                        },
                                        // 委托
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight= 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Commission.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = false,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.IsExtraTaskRewardReceived? "√" : $"{info.FinishedTaskNum}/{info.TotalTaskNum}",
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                },
                                            }
                                        },
                                        // 周本
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 2,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = "resource/others/Domain.png",
                                                    HintAlign = AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = false,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = $"{info.RemainResinDiscountNum}/{info.ResinDiscountNumLimit}",
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        }
                                    }
                                },
                                new AdaptiveText(),
                                // 派遣
                                new AdaptiveGroup
                                {
                                    Children =
                                    {
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[0].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[0].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[1].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[1].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[2].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[2].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[3].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[3].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                        new AdaptiveSubgroup
                                        {
                                            HintWeight = 1,
                                            Children =
                                            {
                                                new AdaptiveImage
                                                {
                                                    Source = info.Expeditions[4].AvatarSideIcon,
                                                    HintAlign= AdaptiveImageAlign.Stretch,
                                                    HintRemoveMargin = true,
                                                },
                                                new AdaptiveText
                                                {
                                                    Text = info.Expeditions[4].RemainedTimeFormatted,
                                                    HintAlign = AdaptiveTextAlign.Center,
                                                }
                                            }
                                        },
                                    }
                                }
                            }
                        }
                    },
                }
            };
        }
    }
}
