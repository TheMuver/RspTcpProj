using System.Linq;
using System.Collections.Generic;
namespace RockScissorsPaperLib
{
    public class AdditionalWinnerChecker
    {
        Dictionary<rsp, rsp[]> d = new Dictionary<rsp, rsp[]>();

        public AdditionalWinnerChecker()
        {
            d.Add(rsp.Lizard, new rsp[] {rsp.Paper, rsp.Spoke});
            d.Add(rsp.Paper, new rsp[] {rsp.Rock, rsp.Spoke});
            d.Add(rsp.Rock, new rsp[] {rsp.Lizard, rsp.Scissors});
            d.Add(rsp.Scissors, new rsp[] {rsp.Lizard, rsp.Paper});
            d.Add(rsp.Spoke, new rsp[] {rsp.Rock, rsp.Scissors});
        }
        public int Play(rsp first, rsp second)
        {
            if((int)first <= 5 && (int)second <= 5)
            {
                if (first == second )
                    return 0;
                else if(d[first].Contains<rsp>(second))
                    return 1;
                else
                    return 2;
            }


            return -1;
        }
    }
}