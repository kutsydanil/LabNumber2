using ConsoleLab2.Interface;
using ConsoleLab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleLab2
{
    public class Program
    {

        private static IGenericRepository<Actors> actorsRepository;
        private static IGenericRepository<ActorCasts> actorCastsRepository;
        private static IGenericRepository<FilmProductions> filmProductionsRepository;
        private static IGenericRepository<Genres> genresRepository;
        private static IGenericRepository<CinemaHalls> cinemaHallsRepository;
        private static IGenericRepository<Staffs> staffsRepository;
        private static IGenericRepository<StaffCasts> staffCastsRepository;
        private static IGenericRepository<CountryProductions> countryProductionsRepository;
        private static IGenericRepository<Films> filmsRepository;
        private static IGenericRepository<Places> placesRepository;
        private static IGenericRepository<ListEvents> listEventsRepository;

        public static void Main(String[] args)
        {
            bool ifInit = true; 
            try
            {
                actorsRepository = new GenericRepository<Actors>();
                actorCastsRepository = new GenericRepository<ActorCasts>();
                filmProductionsRepository = new GenericRepository<FilmProductions>();
                genresRepository = new GenericRepository<Genres>();
                cinemaHallsRepository = new GenericRepository<CinemaHalls>();
                staffsRepository = new GenericRepository<Staffs>();
                staffCastsRepository = new GenericRepository<StaffCasts>();
                countryProductionsRepository = new GenericRepository<CountryProductions>();
                filmsRepository = new GenericRepository<Films>();
                placesRepository = new GenericRepository<Places>();
                listEventsRepository = new GenericRepository<ListEvents>();
                ifInit = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(ifInit);

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Меню: ");
                Console.WriteLine("1 - Выборка из таблиц отношения <Один>");
                Console.WriteLine("2 - Выборка из таблиц отношения <Один> с некоторым фильтром");
                Console.WriteLine("3 - Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового результата (min, max, avg, сount или др.) по выбранному полю из таблицы, стоящей в схеме базы данных нас стороне отношения <Многие>");
                Console.WriteLine("4 - Выборку данных из двух полей двух таблиц, связанных между собой отношением <Один-ко-Многим>.");
                Console.WriteLine("5 - Выборку данных из двух таблиц, связанных между собой отношением <Один-ко-Многим> и отфильтрованным по некоторому условию, налагающему ограничения на значения одного или нескольких полей.");
                Console.WriteLine("6 - Вставку данных в таблицы, стоящей на стороне отношения <Один>.");
                Console.WriteLine("7 - Вставку данных в таблицы, стоящей на стороне отношения <Многие>.");
                Console.WriteLine("8 - Удаление данных из таблицы, стоящей на стороне отношения <Один>.");
                Console.WriteLine("9 - Удаление данных из таблицы, стоящей на стороне отношения <Многие>.");
                Console.WriteLine("10 - Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных.");
                Console.WriteLine("11 - Конец.");
                Console.WriteLine("Введите пункт меню: ");
                
                if(int.TryParse(Console.ReadLine(), out var number) == true)
                {
                    if (ifInit)
                    {
                        switch (number)
                        {
                            case 1:
                                Console.Clear();
                                Console.WriteLine("Пункт 1 - таблица Актеры");
                                Console.WriteLine(Connection.GetConnetion());
                                var actors1 = actorsRepository.GetAll();
                                foreach (var actor in actors1)
                                {
                                    Console.WriteLine(actor.ToString());
                                }
                                break;
                            case 2:
                                Console.Clear();
                                Console.WriteLine("Пункт 2 - таблица Актеры");
                                var actors2 = actorsRepository.GetAll().Where(actor => actor.MiddleName == "Александрович");
                                foreach (var actor in actors2)
                                {
                                    Console.WriteLine(actor.ToString());
                                }
                                break;
                            case 3:
                                Console.Clear();
                                Console.WriteLine("Пункт 3 - таблица Фильмы(6)");
                                var films1 = filmsRepository.GetAll();
                                var genres1 = genresRepository.GetAll();
                                /*var groups = films1.GroupBy(f => f.GenreId).Select(g => new { GenreCount = g.Count()})*/;
                                var results = from film in films1
                                              join genre in genres1 on
                                              film.GenreId equals genre.Id
                                              group genre by new {genre.Name} into grp
                                              select new { Name = grp.Key.Name, Count = grp.Count() };

                                /*foreach (var result in groups)
                                {
                                    Console.WriteLine($"GenreName : {result.GenreCount}");
                                }*/

                                foreach (var result in results)
                                {
                                    Console.WriteLine($"{result.Name} : {result.Count}"); 
                                }

                                break;
                            case 4:
                                Console.Clear();
                                Console.WriteLine("Пункт 4 - таблица Жанры и Фильмы");
                                var films = filmsRepository.GetAll();
                                var genres = genresRepository.GetAll();
                                var results1 = films.Join(genres, f => f.GenreId, g => g.Id,
                                    (f, g) => new {f.Id, f.Name, f.ActorCastId, f.CountryProductionId, f.FilmProductionId, FilmGenre = g.Name, f.AgeLimit }).Take(50);
                                foreach (var result in results1)
                                {
                                    Console.WriteLine($"Id: {result.Id} Name: {result.Name} -- Casts: {result.ActorCastId} -- Country: {result.CountryProductionId} -- Prod: {result.FilmProductionId} -- Genre: {result.FilmGenre} -- AgeLimit: {result.AgeLimit}");
                                }
                                break;
                            case 5:
                                Console.Clear();
                                Console.WriteLine("Пункт 5 - таблица Жанры и Фильмы");
                                films = filmsRepository.GetAll();
                                genres = genresRepository.GetAll();
                                var results2 = films.Join(genres, f => f.GenreId, g => g.Id,
                                    (f, g) => new { FilmName = f.Name, FilmGenre = g.Name, FilmAgeLimit = f.AgeLimit }).Where(g => g.FilmGenre == "Боевик");
                                foreach (var result in results2)
                                {
                                    Console.WriteLine($"FilmName: {result.FilmName} -- Genre: {result.FilmGenre} -- AgeLimit: {result.FilmAgeLimit}");
                                }
                                break;
                            case 6:
                                Console.Clear();
                                Console.WriteLine("Пункт 6 - таблица Актеры");
                                actorsRepository.Create(new Actors("Данилка" , "Куцый" , "Николаевич"));
                                actorsRepository.Save();
                                actors1 = actorsRepository.GetAll();
                                foreach (var actor in actors1)
                                {
                                    Console.WriteLine(actor.ToString());
                                }
                                break;
                            case 7:
                                Console.Clear();
                                Console.WriteLine("Пункт 7 - таблица Актерский состав");
                                actorCastsRepository.Create(new ActorCasts(7 , 8));
                                actorCastsRepository.Save();
                                var actorcasts = actorCastsRepository.GetAll().Reverse().Take(15);
                                foreach (var actorcast in actorcasts)
                                {
                                    Console.WriteLine(actorcast.ToString());
                                }
                                break;
                            case 8:
                                Console.Clear();
                                Console.WriteLine("Пункт 8 - таблица Актеры");
                                var actors3 = actorsRepository.GetAll().Where(actor => actor.Name == "Даниил" && actor.SurName == "Куцый").First();
                                actorcasts = actorCastsRepository.GetAll().Reverse().Where(actorcasts => actorcasts.ActorId == actors3.Id);


                                actorCastsRepository.DeleteByTitem(actorcasts);
                                actorCastsRepository.Save();
                                actorsRepository.DeleteById(actors3.Id);
                                actorsRepository.Save();

                                actors1 = actorsRepository.GetAll();
                                foreach (var actor in actors1)
                                {
                                    Console.WriteLine(actor.ToString());
                                }
                                break;
                            case 9:
                                Console.Clear();
                                Console.WriteLine("Пункт 9 - таблица Актерский состав");
                                var actorcasts2 = actorCastsRepository.GetAll().Where(actorcast => actorcast.ActorId == 7);
                                actorCastsRepository.DeleteByTitem(actorcasts2);
                                actorCastsRepository.Save();
                                actorcasts2 = actorCastsRepository.GetAll();
                                foreach (var actorcast in actorcasts2)
                                {
                                    Console.WriteLine(actorcast.ToString());
                                }
                                break;
                            case 10:
                                Console.Clear();
                                Console.WriteLine("Пункт 10 - таблица Жанры");
                                var genre1 = genresRepository.GetAll().Where(genre => genre.Name == "Комедия").FirstOrDefault();
                                if(genre1 != null)
                                {
                                    genre1.Name = "Комедия2";
                                    genresRepository.Update(genre1);
                                    genresRepository.Save();

                                    var genres2 = genresRepository.GetAll();
                                    foreach (var genre in genres2)
                                    {
                                        Console.WriteLine(genre.ToString());
                                    }
                                }


                                break;
                            case 11:
                                Console.WriteLine("Пункт 11");
                                Console.WriteLine("Закрытие программы....");
                                flag = false;
                                break;
                            default:
                                Console.Clear();
                                Console.WriteLine("Нет такого пункта меню");
                                break;
                        }
                    }
                }
               
            }
        }

    }
        
}
