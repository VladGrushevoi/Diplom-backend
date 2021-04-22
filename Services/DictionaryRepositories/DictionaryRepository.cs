using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Services.DictionaryRepositories
{
    public class DictionaryRepository : BaseRepository, IDictionaryRepository
    {
        public DictionaryRepository(DbAppContext context) : base(context)
        {
        }

        public async Task<List<District>> GetDistrics()
        {
            return await context.districts.ToListAsync();
        }

        public async Task<List<TypePlace>> GetTypePlaces()
        {
            return await context.typePlaces.ToListAsync();
        }

        public async void InitTypePlaces()
        {
            List<TypePlace> typePlaces = new List<TypePlace>(){
                new TypePlace{Name = "Торговий центр"},
                new TypePlace{Name = "Зупинка громадського транспорту"},
                new TypePlace{Name = "Головна дорога"},
                new TypePlace{Name = "Адміністративна будівля"},
                new TypePlace{Name = "Школа"},
                new TypePlace{Name = "Садочок"},
                new TypePlace{Name = "Навчальний заклад"},
                new TypePlace{Name = "Продуктовий магазин"},
                new TypePlace{Name = "Інше"},
            };
            await context.typePlaces.AddRangeAsync(typePlaces);

            context.SaveChanges();
        }

        public async void InitDistrics()
        {
               List<string> districts = new List<string>(){
                "700-летия","Благовесный","Богдановский","Водоконал-Невского","Грузовой порт",
                "Дахновка","Днепровский","Железнодорожний вокзал","Зеленый","к-т Мир","Казбет",
                "Калиновский","Крываловский","Луначарский","Молокозавод","Мытница","Мытница-речпорт",
                "Мытница-центр","Пацаева","Победа","Приднепровский","Припортовый","Пятихатки","Район Д",
                "Самолет","Седова","Соборный","Сосновка","Сосновский","Стадион","Химпоселок","Центр",
                "Черкасский","Школьная","ЮЗР",
            };
            foreach (var item in districts)
            {
                await context.districts.AddAsync(new District(){Name = item});
            }
            await context.SaveChangesAsync();
        }
    }
}