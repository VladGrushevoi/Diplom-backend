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
                new TypePlace{Name = "Торговий центр", Count = 1},
                new TypePlace{Name = "Зупинка громадського транспорту", Count = 5},
                new TypePlace{Name = "Головна дорога", Count = 1},
                new TypePlace{Name = "Адміністративна будівля", Count = 1},
                new TypePlace{Name = "Школа", Count = 2},
                new TypePlace{Name = "Садочок", Count = 2},
                new TypePlace{Name = "Навчальний заклад", Count = 1},
                new TypePlace{Name = "Продуктовий магазин", Count = 5},
                new TypePlace{Name = "Заклад харчування", Count = 4},
                new TypePlace{Name = "Банк", Count = 2},
                new TypePlace{Name = "Медична устонова", Count = 2},
                new TypePlace{Name = "Автобусний вокзал", Count = 1},
                new TypePlace{Name = "ЖД вокзал", Count = 1},
                new TypePlace{Name = "Авто заправка",  Count = 1},
                new TypePlace{Name = "Парк", Count = 1},
                new TypePlace{Name = "Пляж", Count = 1},
                new TypePlace{Name = "Цвинтар", Count = 1},
                new TypePlace{Name = "Інше", Count = 1},
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