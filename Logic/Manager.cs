using DataStructures;
using System;
using System.Linq;
using System.Text;

using Timer = System.Threading.Timer;



namespace Logic
{
    public class Manager
    {
        int maxBoxes = Properties.Settings.Default.maxBoxes;
        int allert = Properties.Settings.Default.allert;
        Timer timer;
        BST<BoxBase> storage = new BST<BoxBase>();
        DoubleLinkedLists<BoxDateDlete> ExpOrderList = new DoubleLinkedLists<BoxDateDlete>();
        BoxHeight BoxHeightY;
        BoxBase boxBaseX;
        BoxHeight BoxHeightYToDelete;
        BoxBase boxBaseXToDelete;
        BoxBase realBoxBaseX;
        int countSplits = 0;
        readonly string space = "================================================== \n";

        public Manager()
        {
            TimeSpan checkPeriod = new TimeSpan(24, 0, 0);
            TimeSpan expiration = new TimeSpan(30, 0, 0, 0);
            timer = new Timer(CheckExpiredBoxes, null, expiration, checkPeriod);
        }

        public void CheckExpiredBoxes(object state)
        {
            foreach (var item in ExpOrderList)
            {
                TimeSpan exp = (DateTime.Now - item.BoxHeightProp.supllyDate);
                TimeSpan expDays = new TimeSpan(30, 0, 0);
                if (exp > expDays)
                {
                    ExpOrderList.RemoveFirst();
                    storage.Search(item.BoxBasepProp, out boxBaseXToDelete);
                    BoxHeightYToDelete = boxBaseXToDelete.boxHeightslist.Find(item.BoxHeightProp).Value;
                    boxBaseXToDelete.boxHeightslist.Remove(BoxHeightYToDelete);
                }
                else return;
            }

        }
       
        bool isStorageEmpty()
        {
            if (storage.isEmpty())
                return true;
            else return false;
        }
        public void Supply(double bBase, double bHeight, int amount, Func<string, bool> askSup, Action<string> PrintMsgSup)
        {
            boxBaseX = new BoxBase(bBase);
            BoxHeightY = new BoxHeight { Y = bHeight, Amount = amount, supllyDate = DateTime.Now };
            if (storage.Search(boxBaseX, out realBoxBaseX))
            {
                BoxHeight box = realBoxBaseX.boxHeightslist.FirstOrDefault(item => item.Y == bHeight);
                if (box != default)
                {
                    box.Amount += amount;
                    box.supllyDate = DateTime.Now;
                    ExpOrderList.AddLast(new BoxDateDlete(realBoxBaseX, box));
                    if (box.Amount > maxBoxes)
                    {
                        int returned = (box.Amount - maxBoxes);
                        box.Amount = maxBoxes;

                        PrintMsgSup($"weve returned you {returned} boxes, sorry");
                    }
                }
                else
                {
                    realBoxBaseX.boxHeightslist.AddFirst(BoxHeightY);

                    if (amount < maxBoxes)
                    {
                        BoxHeightY.Amount = amount;
                    }
                    else
                    {
                        BoxHeightY.Amount = maxBoxes;
                        PrintMsgSup($"weve returned you {amount - BoxHeightY.Amount} boxes, sorry");
                    }
                    BoxHeightY.supllyDate = DateTime.Now;
                    ExpOrderList.AddLast(new BoxDateDlete(realBoxBaseX, BoxHeightY));
                }
                PrintMsgSup(Msg());

            }
            else
            {
                if (amount > maxBoxes)
                {
                    BoxHeightY.Amount = maxBoxes;
                    PrintMsgSup($"weve returned you {amount - BoxHeightY.Amount} boxes, sorry");
                }
                else
                    BoxHeightY.Amount = amount;

                storage.Add(boxBaseX);
                boxBaseX.boxHeightslist.AddFirst(BoxHeightY);

                ExpOrderList.AddLast(new BoxDateDlete(boxBaseX, BoxHeightY));
                PrintMsgSup(Msg());

            }
            BoxHeightY.supllyDate = DateTime.Now;
        }

        public void Buy(double bBase, double bHeight, int amount, Func<string, bool> askBuyer, Action<string> PrintMsg)
        {
            TimeSpan maxDif = new TimeSpan(3, 0, 0);
            int dif = 0;
            BoxBase resBoxBase;
            BoxHeight res;
            if (isStorageEmpty())
                PrintMsg("we have nothing for sale now. sorry. come back later.");
            boxBaseX = new BoxBase(bBase);
            BoxHeightY = new BoxHeight { Y = bHeight, Amount = amount, lastTimeSold = DateTime.Now };

            if (storage.Search(boxBaseX, out resBoxBase))
            {
                BoxHeight box = resBoxBase.boxHeightslist.FirstOrDefault(item => item.Y == BoxHeightY.Y);
                if (box != null)
                {
                    bool answer = askBuyer($"weve found you a perfect box size: {bBase}, {bHeight}. would you like to purchase?");
                    if (answer == true)
                    {
                        box.Amount -= amount;
                        if (box.Amount <= 0)
                        {
                            dif = 0 + (box.Amount * (-1));
                            box.Amount = 0;
                        }
                        if (box.Amount == 0)
                        {
                            resBoxBase.boxHeightslist.Remove(box);
                            PrintMsg("The box is no longer available");
                        }
                        if (Alert(box.Amount)) PrintMsg($"{space} notice that this box's stock is about to end \n {space}");
                        box.lastTimeSold = DateTime.Now;
                        
                    }

                }
                else
                {
                    res = resBoxBase.boxHeightslist.FirstOrDefault(item => item.Y < bHeight * 1.2);
                    if (res != null)
                    {
                        bool answer = askBuyer($"weve found you a box size:{bBase}, {res.Y}. would you like to purchase?");
                        if (answer == true)
                        {
                            res.Amount -= dif;
                            if (res.Amount <= 0)
                            {
                                dif = 0 + (box.Amount * (-1));
                                res.Amount = 0;
                            }
                            if (res.Amount == 0)
                            {
                                resBoxBase.boxHeightslist.Remove(BoxHeightY);
                                PrintMsg("The box is no longer available");

                            }
                            if (Alert(res.Amount)) PrintMsg($"{space} notice that this box's stock is about to end \n {space}");

                            res.lastTimeSold = DateTime.Now;
                        }

                    }
                    if (res == null)
                        PrintMsg("there is no box the size you wanted. sorry.");
                }

            }
            else
            {
                if (storage.SearchClosest(boxBaseX, out realBoxBaseX))
                {
                    if (realBoxBaseX.X < (boxBaseX.X*1.2))
                    {
                        BoxHeight box = realBoxBaseX.boxHeightslist.Find(BoxHeightY)?.Value;
                        if (box != null)
                        {
                            bool answer = askBuyer($"weve found you a  box size:{bBase}, {bHeight}. would you like to purchase?");
                            if (answer == true)
                            {
                                box.Amount -= amount;
                                if (box.Amount <= 0)
                                {
                                    dif = 0 + (box.Amount * (-1));
                                    box.Amount = 0;
                                }
                                if (box.Amount == 0)
                                {
                                    realBoxBaseX.boxHeightslist.Remove(BoxHeightY);
                                    PrintMsg("The box is no longer available");


                                }
                                if (Alert(box.Amount)) PrintMsg($"{space} notice that this box's stock is about to end \n {space}");

                                box.lastTimeSold = DateTime.Now;
                            }
                        }
                        else
                        {
                            res = realBoxBaseX.boxHeightslist.First(item => item?.Y < bHeight * 1.2);
                            if (res != null)
                            {
                                bool answer = askBuyer($"weve found you a box size:{realBoxBaseX.X}, {res.Y}. would you like to purchase?");
                                if (answer == true)
                                {
                                    res.Amount -= amount;
                                    if (res.Amount <= 0)
                                    {
                                        dif = 0 + (res.Amount * (-1));
                                        res.Amount = 0;
                                    }
                                    if (res.Amount == 0)
                                    {
                                        realBoxBaseX.boxHeightslist.Remove(BoxHeightY);
                                        PrintMsg("The box is no longer available");

                                    }
                                    if (Alert(res.Amount)) PrintMsg($"{space} notice that this box's stock is about to end \n {space}");

                                    res.lastTimeSold = DateTime.Now;
                                }

                            }
                            if (res == null)
                                PrintMsg("there is no box the size you wanted. sorry.");
                        }
                    }
                    else
                    {
                        if (countSplits>=1)
                            PrintMsg("We have no more boxes to show you other than what you have bought. sorry");
                        
                        if (countSplits==0)
                        PrintMsg("there is no box the size you wanted. sorry.");
                    }
                }
            }
            if (dif == 0) return;

            countSplits++;

            if (countSplits > 3) return;

            Buy(bBase, bHeight, dif, askBuyer, PrintMsg);
        }

        private string Msg() // O(n)
        {
            StringBuilder sb = new StringBuilder();
            string str = "There are now: \n ";
            foreach (var item in ExpOrderList)
            {
                sb.Append($"{item.BoxHeightProp.Amount} boxes \t with base {item.BoxBasepProp.X} \t and height {item.BoxHeightProp.Y}  \n");
            }
            return space + str + sb.ToString() + space;
        }

        private bool Alert(int amount)
        {
            if (amount <= allert)
                return true;
            else return false;
        }

    }
}
