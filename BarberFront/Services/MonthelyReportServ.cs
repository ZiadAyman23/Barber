using Core.ModelView;

namespace BarberFront.Services
{
    public class MonthelyReportServ
    {
        private readonly IHttpClientFactory httpClientFactory;

        public MonthelyReportServ(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task<MonthelyReportMV> GenratMonthReport(int month, int year)
        {

            var client = httpClientFactory.CreateClient("API");
            var respons = await client.GetAsync($"api/MonthelyReport/monthelyReport?Month={month}&Year={year}");
            if (respons.IsSuccessStatusCode)
            {
                var report = await respons.Content.ReadFromJsonAsync<MonthelyReportMV>();
                return report?? new MonthelyReportMV();
            }
            else
            {
                return new MonthelyReportMV();
            }

        }

    }
}
