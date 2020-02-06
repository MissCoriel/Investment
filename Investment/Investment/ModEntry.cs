using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    public class ModEntry : Mod
    {
        public SDate currentDate;
        public int moneyInvested;
        public int JojaShare;
        public int StardropShare;
        public int SauceShare;
        public int FerngillShare;
        public int JojaPrice;
        public int StardropPrice;
        public int SaucePrice;
        public int FerngillPrice;
        public int JojaModifier = 0;
        
        

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += this.DayStarted;
            helper.Events.GameLoop.SaveLoaded += this.SaveLoaded;
            helper.Events.GameLoop.SaveCreated += this.SaveCreated;
            this.currentDate = SDate.Now();
            helper.ConsoleCommands.Add("stockprice", "show the current stock prices", stockpriceCommand);
            helper.ConsoleCommands.Add("buystock", "usage: buystock <stock> <amount>, purchase stocks. (joja, stardrop, sauce, ferngill)", BuystockCommand);
            helper.ConsoleCommands.Add("sellstock", "usage: sellstock <stock> <amount>, sell stocks. (joja, stardrop, sauce, ferngill)", sellstockCommand);
            

        }
        private void SaveCreated(object sender, SaveCreatedEventArgs e) //When save is created; Set save data for investments
        {
            this.Helper.Data.WriteSaveData("InvestJoja", new SaveModel { InvestJoja = 0 });
            this.Helper.Data.WriteSaveData("InvestStardrop", new SaveModel { InvestStardrop = 0 });
            this.Helper.Data.WriteSaveData("InvestSauce", new SaveModel { InvestSauce = 0 });
            this.Helper.Data.WriteSaveData("InvestFerngill", new SaveModel { InvestFerngill = 0 });
            Monitor.Log($" New Save! No Stocks!", LogLevel.Trace);
            this.Helper.Data.WriteSaveData("TotalInvested", new SaveModel { TotalInvested = 0 });
            Monitor.Log("New Save!  Setting Investment to 0!", LogLevel.Trace);
            this.Helper.Data.WriteSaveData("PriceJoja", new SaveModel { PriceJoja = 250 });
            this.Helper.Data.WriteSaveData("PriceStardrop", new SaveModel { PriceStardrop = 100 });
            this.Helper.Data.WriteSaveData("PriceSauce", new SaveModel { PriceSauce = 50 });
            this.Helper.Data.WriteSaveData("PriceFerngill", new SaveModel { PriceFerngill = 1000 });
            Monitor.Log("New Save!  Base Prices added!", LogLevel.Info);
            this.Helper.Data.WriteSaveData("JojaMod", new SaveModel { JojaMod = false });
        }
        private void stockpriceCommand(string command, string[] parameters)
        {
            var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
            var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
            var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
            var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
            var invst = this.Helper.Data.ReadSaveData<SaveModel>("TotalInvested");
            var prjoja = this.Helper.Data.ReadSaveData<SaveModel>("PriceJoja");
            var prstar = this.Helper.Data.ReadSaveData<SaveModel>("PriceStardrop");
            var prsus = this.Helper.Data.ReadSaveData<SaveModel>("PriceSauce");
            var prfrn = this.Helper.Data.ReadSaveData<SaveModel>("PriceFerngill");
            JojaPrice = prjoja.PriceJoja;
            StardropPrice = prstar.PriceStardrop;
            SaucePrice = prsus.PriceSauce;
            FerngillPrice = prfrn.PriceFerngill;
            

            if (invst == null)
            {
                int invstNullSave = (svJoja.InvestJoja * prjoja.PriceJoja) + (svStar.InvestStardrop * prstar.PriceStardrop) + (svSus.InvestSauce * prsus.PriceSauce) + (svFrn.InvestFerngill + prfrn.PriceFerngill);
                invst = new SaveModel { TotalInvested = invstNullSave };
                this.Helper.Data.WriteSaveData("JojaMod", invst);
                Monitor.Log("Null found in Total Investment Variable.. Set to 0!", LogLevel.Debug);
            }
            if (svJoja == null)
            {
                JojaShare = 0;
            }
            else
            {
                JojaShare = svJoja.InvestJoja;
            }
            if (svStar == null)
            {
                StardropShare = 0;
            }
            else
            {
                StardropShare = svStar.InvestStardrop;
            }
            if (svSus == null)
            {
                SauceShare = 0;
            }
            else
            {
                SauceShare = svSus.InvestSauce;
            }
            if (svFrn == null)
            {
                FerngillShare = 0;
            }
            else
            {
                FerngillShare = svFrn.InvestFerngill;
            }
            Monitor.Log($"JojaCorp: {JojaPrice}g, StardropINC: {StardropPrice}g, SauceINC: {SaucePrice}g, Ferngill: {FerngillPrice}g", LogLevel.Info);
            Monitor.Log($"You have {JojaShare} JojaCorp, {StardropShare} StardropINC, {SauceShare} SauceINC, {FerngillShare} Ferngill shares.", LogLevel.Info);
            Monitor.Log($"You have invested {moneyInvested}g in shares and hold a value of {(JojaPrice * JojaShare) + (StardropPrice * StardropShare) + (SaucePrice * SauceShare) + (FerngillPrice * FerngillShare)}g in Total Stock Value!", LogLevel.Info);
        }
        private void BuystockCommand(string command, string[] parameters)
        {           
            if (parameters.Length == 0) return;
            try
            {
                var invst = this.Helper.Data.ReadSaveData<SaveModel>("TotalInvested");
                if (invst == null)
                {
                    var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
                    var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
                    var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
                    var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");                   
                    var prjoja = this.Helper.Data.ReadSaveData<SaveModel>("PriceJoja");
                    var prstar = this.Helper.Data.ReadSaveData<SaveModel>("PriceStardrop");
                    var prsus = this.Helper.Data.ReadSaveData<SaveModel>("PriceSauce");
                    var prfrn = this.Helper.Data.ReadSaveData<SaveModel>("PriceFerngill");
                    int invstNullSave = (svJoja.InvestJoja * prjoja.PriceJoja) + (svStar.InvestStardrop * prstar.PriceStardrop) + (svSus.InvestSauce * prsus.PriceSauce) + (svFrn.InvestFerngill + prfrn.PriceFerngill);
                    invst = new SaveModel { TotalInvested = invstNullSave };
                    this.Helper.Data.WriteSaveData("JojaMod", invst);
                    Monitor.Log("Null found in Total Investment Variable.. Calculating compared to today's prices", LogLevel.Debug);
                }

                int Invested = invst.TotalInvested;
                string stonks = parameters[0];
                int amount = int.Parse(parameters[1]);
                if (string.Equals(stonks, "joja"))
                {
                    int TotalPrice = JojaPrice * amount;
                    if (Game1.player.Money >= TotalPrice)
                    {
                        var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
                        if (svJoja == null)
                        {
                            JojaShare = 0;
                        }
                        else
                        {
                            JojaShare = svJoja.InvestJoja + amount;
                        }
                        
                        Game1.player.Money -= TotalPrice;
                        moneyInvested = moneyInvested + TotalPrice;
                        this.Helper.Data.WriteSaveData("InvestJoja", new SaveModel { InvestJoja = JojaShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested + TotalPrice });
                        Monitor.Log($"Successfully purchased {amount} shares in JojaCorp for {TotalPrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("This Is Not Enough Golds!", LogLevel.Info);
                    }
                    
                }
                if (string.Equals(stonks, "stardrop"))
                {
                    int TotalPrice = StardropPrice * amount;
                    if (Game1.player.Money >= TotalPrice)
                    {
                        var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
                        if (svStar == null)
                        {
                            StardropShare = 0;
                        }
                        else
                        {
                            StardropShare = svStar.InvestStardrop + amount;
                        }
                        
                        Game1.player.Money -= TotalPrice;
                        moneyInvested = moneyInvested + TotalPrice;
                        this.Helper.Data.WriteSaveData("InvestStardrop", new SaveModel { InvestStardrop = StardropShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested + TotalPrice });
                        Monitor.Log($"Successfully purchased {amount} shares in StardropINC for {TotalPrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("This Is Not Enough Golds!", LogLevel.Info);
                    }
                }
                if (string.Equals(stonks, "sauce"))
                {
                    int TotalPrice = SaucePrice * amount;
                    int CurrentCash = Game1.player.Money;
                    Monitor.Log($"Total Purchase price {TotalPrice} and you have {Game1.player.Money}", LogLevel.Debug);
                    if (CurrentCash >= TotalPrice)
                    {
                        var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
                        if (svSus == null)
                        {
                            SauceShare = 0 + amount;
                        }
                        else
                        {
                            SauceShare = svSus.InvestSauce + amount;
                        }
                        Game1.player.Money -= TotalPrice;
                        moneyInvested = moneyInvested + TotalPrice;
                        this.Helper.Data.WriteSaveData("InvestSauce", new SaveModel { InvestSauce = SauceShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested + TotalPrice });
                        Monitor.Log($"Successfully purchase {amount} shares in Queen of Sauce INC for {TotalPrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("This Is Not Enough Golds!", LogLevel.Info);
                    }
                }
                if (string.Equals(stonks, "ferngill"))
                {
                    int TotalPrice = FerngillPrice * amount;
                    if (Game1.player.Money >= TotalPrice)
                    {
                        var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
                        if (svFrn == null)
                        {
                            FerngillShare = 0;
                        }
                        else
                        {
                            FerngillShare = svFrn.InvestFerngill + amount;
                        }
                        
                        Game1.player.Money -= TotalPrice;
                        moneyInvested = moneyInvested + TotalPrice;
                        this.Helper.Data.WriteSaveData("InvestFerngill", new SaveModel { InvestFerngill = FerngillShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested + TotalPrice });
                        Monitor.Log($"Successfully purchase {amount} shares in Bank of Ferngill for {TotalPrice}g!");
                    }
                    else
                    {
                        Monitor.Log("This Is Not Enough Golds!", LogLevel.Info);
                    }
                }
                

            }
            catch (Exception) { }
        }
        private void sellstockCommand(string command, string[] parameters)
        {
            
            if (parameters.Length == 0) return;
            try
            {
                var invst = this.Helper.Data.ReadSaveData<SaveModel>("TotalInvested");
                if (invst == null)
                {
                    var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
                    var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
                    var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
                    var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
                    var prjoja = this.Helper.Data.ReadSaveData<SaveModel>("PriceJoja");
                    var prstar = this.Helper.Data.ReadSaveData<SaveModel>("PriceStardrop");
                    var prsus = this.Helper.Data.ReadSaveData<SaveModel>("PriceSauce");
                    var prfrn = this.Helper.Data.ReadSaveData<SaveModel>("PriceFerngill");
                    int invstNullSave = (svJoja.InvestJoja * prjoja.PriceJoja) + (svStar.InvestStardrop * prstar.PriceStardrop) + (svSus.InvestSauce * prsus.PriceSauce) + (svFrn.InvestFerngill + prfrn.PriceFerngill);
                    invst = new SaveModel { TotalInvested = invstNullSave };
                    this.Helper.Data.WriteSaveData("JojaMod", invst);
                    Monitor.Log("Null found in Total Investment Variable.. Calculating compared to today's prices", LogLevel.Debug);
                }


                int Invested = invst.TotalInvested;
                string stonks = parameters[0];
                int amount = int.Parse(parameters[1]);
               // Monitor.Log($"Parameters {stonks} and {amount}", LogLevel.Debug);
                if (string.Equals(stonks, "joja"))
                {
                    var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
                    if (svJoja == null)
                    {
                        JojaShare = 0;
                        Monitor.Log($"Null - Set to 0 shares", LogLevel.Debug);
                    }
                    else
                    {
                        JojaShare = svJoja.InvestJoja;
                        Monitor.Log($"{JojaShare} shares", LogLevel.Debug);
                    }
                    
                    if (amount <= JojaShare)
                    {
                        JojaShare = JojaShare - amount;
                        Game1.player.Money += amount * JojaPrice;
                        this.Helper.Data.WriteSaveData("InvestJoja", new SaveModel { InvestJoja = JojaShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested - (amount * JojaPrice) });
                        Monitor.Log($"Successfully sold {amount} shares for JojaCorp for {JojaPrice} each, totaling {amount * JojaPrice}g!", LogLevel.Info);

                    }
                    else
                    {
                        Monitor.Log("You've not enough Shares!", LogLevel.Info);
                    }
                }
                if (string.Equals(stonks, "stardrop"))
                {
                    var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
                    StardropShare = svStar.InvestStardrop;
                    if (amount >= StardropShare)
                    {
                        StardropShare = StardropShare - amount;
                        Game1.player.Money += amount * StardropPrice;
                        this.Helper.Data.WriteSaveData("InvestStardrop", new SaveModel { InvestStardrop = StardropShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested - (amount * StardropPrice) });
                        Monitor.Log($"Successfully sold {amount} shares for StardropINC for {StardropPrice} each, totaling {amount * StardropPrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("You've not enough Shares!", LogLevel.Info);
                    }
                }
                if (string.Equals(stonks, "sauce"))
                {
                    var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
                    SauceShare = svSus.InvestSauce;
                    if (amount <= SauceShare)
                    {
                        SauceShare = SauceShare - amount;
                        Game1.player.Money += amount * SaucePrice;
                        this.Helper.Data.WriteSaveData("InvestSauce", new SaveModel { InvestSauce = SauceShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested - (amount * SaucePrice) });
                        Monitor.Log($"Successfully sold {amount} shares for Queen of Sauce INC for {SaucePrice} each, totaling {amount * SaucePrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("You've not enough Shares!", LogLevel.Info);
                    }
                }
                if (string.Equals(stonks, "ferngill"))
                {
                    var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
                    FerngillShare = svFrn.InvestFerngill;
                    if (amount <= FerngillShare)
                    {
                        FerngillShare = FerngillShare - amount;
                        Game1.player.Money += amount * FerngillPrice;
                        this.Helper.Data.WriteSaveData("InvestFerngill", new SaveModel { InvestFerngill = FerngillShare });
                        this.Helper.Data.WriteSaveData("TotalInvestment", new SaveModel { TotalInvested = Invested - (amount * FerngillPrice) });
                        Monitor.Log($"Successfully sold {amount} shares for Bank of Ferngill for {FerngillPrice} each, totaling {amount * FerngillPrice}g!", LogLevel.Info);
                    }
                    else
                    {
                        Monitor.Log("You've not enough Shares!", LogLevel.Info);
                    }

                }
            }
            catch(Exception) { }
        }

        public void SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            var jojaCh = this.Helper.Data.ReadSaveData<SaveModel>("JojaMod");
            if (jojaCh == null || !jojaCh.JojaMod)
            {
                jojaCh = new SaveModel { JojaMod = false };
                this.Helper.Data.WriteSaveData("JojaMod", jojaCh);                
                Monitor.Log("Joja Modifier Flag not found.  Set to False!!", LogLevel.Debug);
            }
            var prjoja = this.Helper.Data.ReadSaveData<SaveModel>("PriceJoja");
            if (prjoja == null)
            {
                prjoja = new SaveModel { PriceJoja = 250 };
                this.Helper.Data.WriteSaveData("PriceJoja", prjoja);
                Monitor.Log("Not New Save, but no price for Joja!! Set Base Price!(If you updated, this will be the only time this happens!)", LogLevel.Debug);
            }


            if (jojaCh == null || jojaCh.JojaMod == false || !jojaCh.JojaMod) //This will check if you have helped or hindered JojaMart
            {
                if (Game1.MasterPlayer.mailReceived.Contains("ccIsComplete")) //If you completed the Community Center, Joja Stocks take a -50g hit
                {
                    JojaPrice = prjoja.PriceJoja - 50;
                    Monitor.Log("Due to your work with the Community Center, JojaCorp has taken a drop in Stock Price!", LogLevel.Info);
                    this.Helper.Data.WriteSaveData("JojaMod", new SaveModel { JojaMod = true });
                    this.Helper.Data.WriteSaveData("PriceJoja", new SaveModel { PriceJoja = JojaPrice });
                }
                if (Game1.MasterPlayer.eventsSeen.Contains(502261)) //If made hero by Joja, Joja Stocks jump up +50g
                {
                    JojaPrice = prjoja.PriceJoja + 50;
                    Monitor.Log("Due to your assistance, JojaCorp Stock price has increased!", LogLevel.Info);
                    this.Helper.Data.WriteSaveData("JojaMod", new SaveModel { JojaMod = true });
                    this.Helper.Data.WriteSaveData("PriceJoja", new SaveModel { PriceJoja = JojaPrice });
                }

            }
            var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
            var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
            var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
            var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
            var prstar = this.Helper.Data.ReadSaveData<SaveModel>("PriceStardrop");
            var prsus = this.Helper.Data.ReadSaveData<SaveModel>("PriceSauce");
            var prfrn = this.Helper.Data.ReadSaveData<SaveModel>("PriceFerngill");
            Monitor.Log($"Checking Save Data:", LogLevel.Debug);
            if (prstar == null)
            {
                this.Helper.Data.WriteSaveData("PriceStardrop", new SaveModel { PriceStardrop = 100 });
                Monitor.Log("Not New Save, but no price for Stardrop!! Set Base Price!(If you updated, this will be the only time this happens!)", LogLevel.Debug);
            }
            if (prsus == null)
            {
                this.Helper.Data.WriteSaveData("PriceSauce", new SaveModel { PriceSauce = 50 });
                Monitor.Log("Not New Save, but no price for Sauce!! Set Base Price!(If you updated, this will be the only time this happens!)", LogLevel.Debug);
            }
            if (prfrn == null)
            {
                this.Helper.Data.WriteSaveData("PriceFerngill", new SaveModel { PriceFerngill = 1000 });
                Monitor.Log("Not New Save, but no price for Ferngill!! Set Base Price!(If you updated, this will be the only time this happens!)", LogLevel.Debug);
            }
            if (svJoja == null)
            {
                this.Helper.Data.WriteSaveData("InvestJoja", new SaveModel { InvestJoja = 0 });
                Monitor.Log("Joja data is null, Create Data as zero", LogLevel.Debug);
            }
            if (svStar == null)
            {
                this.Helper.Data.WriteSaveData("InvestStardrop", new SaveModel { InvestStardrop = 0 });
                Monitor.Log("Stardrop data is null, Create Data as zero", LogLevel.Debug);
            }
            if (svSus == null)
            {
                this.Helper.Data.WriteSaveData("InvestSauce", new SaveModel { InvestSauce = 0 });
                Monitor.Log("Sauce data is null, Create Data as zero", LogLevel.Debug);
            }
            if (svFrn == null)
            {
                this.Helper.Data.WriteSaveData("InvestFerngill", new SaveModel { InvestFerngill = 0 });
                Monitor.Log("Ferngill data is null, Create Data as zero", LogLevel.Debug);
            }
        }
        public void DayStarted(object sender, DayStartedEventArgs e)
        {
            var svJoja = this.Helper.Data.ReadSaveData<SaveModel>("InvestJoja");
            var svStar = this.Helper.Data.ReadSaveData<SaveModel>("InvestStardrop");
            var svSus = this.Helper.Data.ReadSaveData<SaveModel>("InvestSauce");
            var svFrn = this.Helper.Data.ReadSaveData<SaveModel>("InvestFerngill");
            var prjoja = this.Helper.Data.ReadSaveData<SaveModel>("PriceJoja");
            var prstar = this.Helper.Data.ReadSaveData<SaveModel>("PriceStardrop");
            var prsus = this.Helper.Data.ReadSaveData<SaveModel>("PriceSauce");
            var prfrn = this.Helper.Data.ReadSaveData<SaveModel>("PriceFerngill");
            //Set Prices
            JojaPrice = prjoja.PriceJoja;
            StardropPrice = prstar.PriceStardrop;
            SaucePrice = prsus.PriceSauce;
            FerngillPrice = prfrn.PriceFerngill;
            Monitor.Log("New Day!  Price Changes set!", LogLevel.Info);

            if (this.currentDate.Day == 1)
            {
                JojaShare = svJoja.InvestJoja;
                if (JojaShare > 0)
                {
                    double jojaInvestment = JojaShare * JojaPrice;
                    jojaInvestment = jojaInvestment * .07;
                    if (jojaInvestment > JojaShare * JojaPrice)
                    {
                        double interest = jojaInvestment - (JojaShare * JojaPrice);
                        Game1.player.Money += (int)interest;
                        Monitor.Log($"This season JojaCorp Shares gave you {interest} in interest!", LogLevel.Info);
                    }
                }
                StardropShare = svStar.InvestStardrop;
                if (StardropShare > 0)
                {
                    double StarInvestment = StardropShare * StardropPrice;
                    StarInvestment = StarInvestment * .05;
                    if (StarInvestment > StardropShare * StardropPrice)
                    {
                        double interest = StarInvestment - (StardropShare * StardropPrice);
                        Game1.player.Money += (int)interest;
                        Monitor.Log($"This season SundropINC Shares gave you {interest} in interest!", LogLevel.Info);
                    }
                }
                SauceShare = svSus.InvestSauce;
                if (SauceShare > 0)
                {
                    double SusInvestment = SauceShare * SaucePrice;
                    SusInvestment = SusInvestment * .03;
                    if (SusInvestment > SauceShare * SauceShare)
                    {
                        double interest = SusInvestment - (SauceShare * SaucePrice);
                        Game1.player.Money += (int)interest;
                        Monitor.Log($"This season Queen of SauceINC Shares gave you {interest} in interest!", LogLevel.Info);
                    }
                }
                FerngillShare = svFrn.InvestFerngill;
                if (FerngillShare > 0)
                {
                    double FernInvestment = FerngillShare * FerngillPrice;
                    FernInvestment = FernInvestment * .15;
                    if (FernInvestment > FerngillShare * FerngillPrice)
                    {
                        double interest = FernInvestment - (FerngillShare * FerngillPrice);
                        Game1.player.Money += (int)interest;
                        Monitor.Log($"This season Bank of Ferngill Shares gave you {interest} in interest!", LogLevel.Info);
                    }
                }
            }
            else
            {
                Random roll = new Random((int)Game1.stats.DaysPlayed + (int)Game1.uniqueIDForThisGame / 2);
                int JojaFluctuation = roll.Next(1, 101);
                int StarDropFluctuation = roll.Next(1, 101);
                int SauceFluctuation = roll.Next(1, 101);
                int FerngillFluctuation = roll.Next(1, 101);
                Monitor.Log($"Joja Roll {JojaFluctuation}", LogLevel.Trace);
                Monitor.Log($"Stardrop Roll {StarDropFluctuation}", LogLevel.Trace);
                Monitor.Log($"Sauce Roll {SauceFluctuation}", LogLevel.Trace);
                Monitor.Log($"Ferngill Roll {FerngillFluctuation}", LogLevel.Trace);
                // 1 is critical fail - 15g 100 is critical success +15 50-99 success +2 02-49 fail -2
                if (JojaFluctuation == 1)
                {
                    if (JojaPrice > 200)
                    {
                        JojaPrice = JojaPrice - 15;
                        Monitor.Log("JojaCorp shares have dropped sharply today!", LogLevel.Debug);
                    }
                }
                if (JojaFluctuation >= 2 && JojaFluctuation <= 49)
                {
                    if (JojaPrice > 200)
                    {
                        JojaPrice = JojaPrice - 2;
                        Monitor.Log("JojaCorp Shares have dropped today!", LogLevel.Debug);
                    }
                }
                if (JojaFluctuation >=50 && JojaFluctuation <= 99)
                {
                    if (JojaPrice < 500)
                    {
                        JojaPrice = JojaPrice + 2;
                        Monitor.Log("JojaCorp Shares have increased today!", LogLevel.Debug);
                    }
                }
                if (JojaFluctuation >= 100)
                {
                    if (JojaPrice < 500)
                    {
                        JojaPrice = JojaPrice + 15;
                        Monitor.Log("JojaCorp shares have sharply increased today!", LogLevel.Debug);
                    }
                }
                if (StarDropFluctuation == 1)
                {
                    if (StardropPrice > 50)
                    {
                        StardropPrice = StardropPrice - 15;
                        Monitor.Log("StardropINC shares have dropped sharply today!", LogLevel.Debug);
                    }
                }
                if (StarDropFluctuation >= 2 && StarDropFluctuation <= 49)
                {
                    if (StardropPrice > 50)
                    {
                        StardropPrice = StardropPrice - 2;
                        Monitor.Log("StardropINC Shares have dropped today!", LogLevel.Debug);
                    }
                }
                if (StarDropFluctuation >= 50 && StarDropFluctuation <= 99)
                {
                    if (StardropPrice < 250)
                    {
                        StardropPrice = StardropPrice + 2;
                        Monitor.Log("StardropINC Shares have increased today!", LogLevel.Debug);
                    }
                }
                if (StarDropFluctuation >= 100)
                {
                    if (StardropPrice < 250)
                    {
                        StardropPrice = StardropPrice + 15;
                        Monitor.Log("StardropINC shares have sharply increased today!", LogLevel.Debug);
                    }
                }
                if (SauceFluctuation == 1)
                {
                    if (SaucePrice > 25)
                    {
                        SaucePrice = SaucePrice - 15;
                        Monitor.Log("Queen of SauceINC shares have dropped sharply today!", LogLevel.Debug);
                    }
                }
                if (SauceFluctuation >= 2 && SauceFluctuation <= 49)
                {
                    if (SaucePrice > 25)
                    {
                        SaucePrice = SaucePrice- 2;
                        Monitor.Log("Queen of SauceINC Shares have dropped today!", LogLevel.Debug);
                    }
                }
                if (SauceFluctuation >= 50 && SauceFluctuation <= 99)
                {
                    if (SaucePrice < 800)
                    {
                        SaucePrice = SaucePrice + 2;
                        Monitor.Log("Queen of SauceINC Shares have increased today!", LogLevel.Debug);
                    }
                }
                if (SauceFluctuation >= 100)
                {
                    if (SaucePrice < 800)
                    {
                        SaucePrice = SaucePrice + 15;
                        Monitor.Log("Queen of SauceINC shares have sharply increased today!", LogLevel.Debug);
                    }
                }
                if (FerngillFluctuation == 1)
                {
                    if (FerngillPrice > 500)
                    {
                        FerngillPrice = FerngillPrice - 15;
                        Monitor.Log("Bank of Ferngill shares have dropped sharply today!", LogLevel.Debug);
                    }
                }
                if (FerngillFluctuation >= 2 && FerngillFluctuation <= 49)
                {
                    if (FerngillPrice > 500)
                    {
                        FerngillPrice = FerngillPrice - 2;
                        Monitor.Log("Bank of Ferngill Shares have dropped today!", LogLevel.Debug);
                    }
                }
                if (FerngillFluctuation >= 50 && FerngillFluctuation <= 99)
                {
                    if (SaucePrice < 3000)
                    {
                        FerngillPrice = FerngillPrice + 2;
                        Monitor.Log("Bank of Ferngill Shares have increased today!", LogLevel.Debug);
                    }
                }
                if (FerngillFluctuation >= 100)
                {
                    if (FerngillPrice < 3000)
                    {
                        FerngillPrice = FerngillPrice + 15;
                        Monitor.Log("Bank of Ferngill shares have sharply increased today!", LogLevel.Debug);
                    }
                }
                this.Helper.Data.WriteSaveData("PriceJoja", new SaveModel { PriceJoja = JojaPrice });
                this.Helper.Data.WriteSaveData("PriceStardrop", new SaveModel { PriceStardrop = StardropPrice });
                this.Helper.Data.WriteSaveData("PriceSauce", new SaveModel { PriceSauce = SaucePrice });
                this.Helper.Data.WriteSaveData("PriceFerngill", new SaveModel { PriceFerngill = FerngillPrice });


            }
        }
       


    }

}
