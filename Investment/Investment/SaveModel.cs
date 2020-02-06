using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investment
{
    class SaveModel
    {
        public int InvestJoja { get; set; } //saves the amount of shares you currently have for Joja
        public int InvestStardrop { get; set; } //saves the amount of shares you currently have for Stardrop
        public int InvestSauce { get; set; } //saves the amount of shares you currently have for Sauce
        public int InvestFerngill { get; set; } //saves the amount of shares you have for Ferngill
        public int TotalInvested { get; set; } //Tracks amount of money you have invested in all the shares
        public int PriceJoja { get; set; } //Tracks the current stock price of joja
        public int PriceStardrop { get; set; } //Tracks the current stock price of Stardrop
        public int PriceSauce { get; set; } //Tracks the current stock price if Sauce
        public int PriceFerngill { get; set; } //Tracks the current stock price of Ferngill
        public bool JojaMod { get; set; } //Ensures the modifier for joja applies only once       
    }
}
