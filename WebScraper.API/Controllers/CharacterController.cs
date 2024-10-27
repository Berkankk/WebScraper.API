using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebScraper.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        //Web sitesinden bilgi çıkarabilmek için o siteye istek atarız ve o istek sonucunda istediğimiz bilgileri ayıklarız 

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var url = "https://dragonball.fandom.com/wiki/Main_Page";

            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            // Tüm 'img' etiketlerini seçiyoruz
            IList<HtmlNode> characterImages = doc.QuerySelectorAll("img");

            // 'alt' özelliğinden karakter isimlerini çekiyoruz
       
            var data = characterImages
                .Where(img => img.GetAttributeValue("alt", "").StartsWith("Cb")) // 'Cb' ile başlayanları filtreliyoruz
                .Select(img => new
                {
                    name = img.GetAttributeValue("alt", "Unknown"), //Bunlar sayfa kaynağını incele dedikten sonra aldık
                    imageUrl = img.GetAttributeValue("data-src",    // Öncelikli olarak 'data-src'yi alıyoruz
                        img.GetAttributeValue("src", "No Image")) // 'src' yedeği varsa onu alıyoruz

                    
                });

            return Ok(data);
        }






    }
}
