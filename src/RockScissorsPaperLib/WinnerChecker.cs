using System.Security.Cryptography;
using System;

namespace RockScissorsPaperLib
{
    public enum rsp { Rock, Scissors, Paper, Lizard, Spoke }

    public class WinnerChecker
    {
        public int Play(rsp first, rsp second)
        {
            switch (first)
            {
                case rsp.Paper:
                    if (second == rsp.Paper)
                        return 0;
                    else if (second == rsp.Rock)
                        return 1;
                    else if (second == rsp.Scissors)
                        return 2;
                    break;
                case rsp.Rock:
                    if (second == rsp.Paper)
                        return 2;
                    else if (second == rsp.Rock)
                        return 0;
                    else if (second == rsp.Scissors)
                        return 1;
                    break;
                case rsp.Scissors:
                    if (second == rsp.Paper)
                        return 1;
                    else if (second == rsp.Rock)
                        return 2;
                    else if (second == rsp.Scissors)
                        return 0;
                    break;
                default:
                    return -1;
                    break;
            }
            return -1;
        }
    }
}
