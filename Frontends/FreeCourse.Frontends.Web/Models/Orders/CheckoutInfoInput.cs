using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Frontends.Web.Models.Orders
{
    public class CheckoutInfoInput
    {
        // Order Information BEGIN
        [Display(Name = "İl")]
        public string Province { get; set; }

        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Display(Name = "Cadde")]
        public string Street { get; set; }

        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }

        [Display(Name = "Adres")]
        public string Line { get; set; }
        // Order Information END


        // Card Information BEGIN

        [Display(Name = "Kart İsim Soyisim")]
        public string CardName { get; set; }

        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }

        [Display(Name = "Son Kullanma Tarihi(Ay/Yıl)")]
        public string Expiration { get; set; }

        [Display(Name = "CVV/CVC")]
        public string CVV { get; set; }

        // Card Information END

    }
}
