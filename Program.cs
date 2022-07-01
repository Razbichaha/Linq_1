using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Linq_1
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramCore programCore = new();
            programCore.StartSearch();
            Console.ReadLine();
        }

        class Renderer
        {
            internal void ShowHeader()
            {
                Console.WriteLine("База преступников\nПараметры для поиска ");
                Console.WriteLine("W - поиск по весу");
                Console.WriteLine("H - поиск по росту");
                Console.WriteLine("N - поиск по национальности");
                Console.WriteLine("Z - отбывает наказание или нет");
                Console.WriteLine("A - показать всю базу");
            }

            internal void ShowStatus()
            {
                string[] status = { "Заключенный", "Освобожден", "Не судим" };

                foreach (string item in status)
                {
                    Console.Write(item + ", ");
                }
                Console.WriteLine();
            }

            internal void SowNationals(Perpetrator perpetrator)
            {
                string[] temp = perpetrator.GetnationalityBase();

                foreach (string item in temp)
                {
                    Console.Write(item + ", ");
                }
            }

            internal void ShowPerpetrator(Perpetrator perpetrator)
            {
                string status = "Заключенный";

                if (perpetrator.Status!=status)
                {
                Console.WriteLine($" {perpetrator.FullName } | Рост - {perpetrator.Height} | Вес - {perpetrator.Weight} |" +
                    $" Национальность - {perpetrator.Nationality} | Статус - {perpetrator.Status}");
                }else
                {
                    Console.WriteLine("Не достаточен уровень допуска");
                }
            }
        }

        class ProgramCore
        {
            private int _quantityPerpetratos = 30;
            private List<Perpetrator> _perpetrators = new List<Perpetrator>();

            private Renderer _renderer = new();

            public ProgramCore()
            {
                GenerateBase();
            }

            internal void StartSearch()
            {
                bool continueSearch = true;

                while (continueSearch)
                {
                    Console.Clear();
                    Console.WriteLine("Для продолжения нажмите Enter");
                    Console.ReadLine();
                    _renderer.ShowHeader();
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.W:

                            SearchByW();

                            break;
                        case ConsoleKey.H:

                            SearchByH();

                            break;
                        case ConsoleKey.N:

                            SearchByN();

                            break;
                        case ConsoleKey.Z:

                            SearchByZ();

                            break;
                        case ConsoleKey.A:

                            SearchByA();

                            break;
                        case ConsoleKey.Escape:

                            continueSearch = false;

                            break;
                    }
                    Console.ReadLine();
                }
            }

            private void SearchByW()
            {
                Console.Clear();
                Console.Write("Введите вес - ");
                string weightString = Console.ReadLine();
                int inputPlaer;

                if (int.TryParse(weightString, out inputPlaer))
                {
                    var weight = from Perpetrator perpetrator in _perpetrators where perpetrator.Weight == inputPlaer select perpetrator;

                    if (weight.Count() == 0)
                    {
                        Perpetrator perpetratorUp = new();
                        Perpetrator perpetratorDown = new();

                        Console.WriteLine("Люди с указанным весом отсутствуют");
                        Console.WriteLine("Ближайшие с указанным весом");

                        FindClosestWeight(inputPlaer, ref perpetratorUp, ref perpetratorDown);
                        _renderer.ShowPerpetrator(perpetratorUp);

                        if (perpetratorUp != perpetratorDown)
                        {
                            _renderer.ShowPerpetrator(perpetratorDown);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ввод не верен");
                }
            }

            private void SearchByH()
            {
                Console.Clear();
                Console.Write("Введите рост - ");
                string heightString = Console.ReadLine();
                int inputPlaer;

                if (int.TryParse(heightString, out inputPlaer))
                {
                    var height = from Perpetrator perpetrator in _perpetrators where perpetrator.Height == inputPlaer select perpetrator;

                    if (height.Count() == 0)
                    {
                        Perpetrator perpetratorUp = new();
                        Perpetrator perpetratorDown = new();

                        Console.WriteLine("Люди с указанным весом отсутствуют");
                        Console.WriteLine("Ближайшие с указанным ростом");

                        FindClosestHeight(inputPlaer, ref perpetratorUp, ref perpetratorDown);
                        _renderer.ShowPerpetrator(perpetratorUp);

                        if (perpetratorUp != perpetratorDown)
                        {
                            _renderer.ShowPerpetrator(perpetratorDown);
                        }
                    }
                }
            }

            private void SearchByN()
            {
                Console.Clear();
                Console.WriteLine("Существующие национальности");
                _renderer.SowNationals(_perpetrators[0]);
                Console.Write("\nВведите национальность - ");
                string inputPlaer = Console.ReadLine();

                var status = from Perpetrator perpetrator in _perpetrators where perpetrator.Nationality == inputPlaer select perpetrator;

                foreach (Perpetrator item in status)
                {
                    _renderer.ShowPerpetrator(item);
                }
            }

            private void SearchByZ()
            {
                Console.Clear();
                Console.WriteLine("Возможный статус");
                _renderer.ShowStatus();
                Console.Write("\nВведите статус - ");
                string inputPlaer = Console.ReadLine();

                var status = from Perpetrator perpetrator in _perpetrators where perpetrator.Status == inputPlaer select perpetrator;

                foreach (Perpetrator item in status)
                {
                    _renderer.ShowPerpetrator(item);
                }
            }

            private void SearchByA()
            {
                Console.WriteLine();

                foreach (Perpetrator perpetrator in _perpetrators)
                {
                    _renderer.ShowPerpetrator(perpetrator);
                }
            }

            private void FindClosestWeight(int weight, ref Perpetrator perpetratorUp, ref Perpetrator perpetratorDown)
            {
                var perpetratorSortingUp = from Perpetrator in _perpetrators where Perpetrator.Weight > weight select Perpetrator;
                var sortingUp = from Perpetrator in perpetratorSortingUp orderby Perpetrator.Weight select Perpetrator;

                if (sortingUp.Count() != 0)
                {
                    perpetratorUp = sortingUp.First();
                }

                var perpetratorSortingDown = from Perpetrator in _perpetrators where Perpetrator.Weight < weight select Perpetrator;
                var sortingDown = from Perpetrator in perpetratorSortingDown orderby Perpetrator.Weight descending select Perpetrator;

                if (sortingDown.Count() == 0)
                {
                    perpetratorDown = perpetratorUp;
                }
                else
                {
                    perpetratorDown = sortingDown.First();
                }

                if (sortingUp.Count() == 0)
                {
                    perpetratorUp = perpetratorDown;
                }
            }

            private void FindClosestHeight(int height, ref Perpetrator perpetratorUp, ref Perpetrator perpetratorDown)
            {
                var perpetratorSortingUp = from Perpetrator in _perpetrators where Perpetrator.Height > height select Perpetrator;
                var sortingUp = from Perpetrator in perpetratorSortingUp orderby Perpetrator.Height select Perpetrator;

                if (sortingUp.Count() != 0)
                {
                    perpetratorUp = sortingUp.First();
                }

                var perpetratorSortingDown = from Perpetrator in _perpetrators where Perpetrator.Height < height select Perpetrator;
                var sortingDown = from Perpetrator in perpetratorSortingDown orderby Perpetrator.Height descending select Perpetrator;

                if (sortingDown.Count() == 0)
                {
                    perpetratorDown = perpetratorUp;
                }
                else
                {
                    perpetratorDown = sortingDown.First();
                }

                if (sortingUp.Count() == 0)
                {
                    perpetratorUp = perpetratorDown;
                }
            }

            private void GenerateBase()
            {
                bool continueGeneration = true;

                while (continueGeneration)
                {
                    Perpetrator perpetrator = new Perpetrator();

                    if (CheckFullNameMatching(perpetrator) == false)
                    {
                        _perpetrators.Add(perpetrator);
                    }
                    if (_perpetrators.Count == _quantityPerpetratos)
                    {
                        continueGeneration = false;
                    }
                }
            }

            private bool CheckFullNameMatching(Perpetrator perpetrator)
            {
                bool isMatching = false;

                foreach (Perpetrator perpetratorTemp in _perpetrators)
                {
                    if (perpetrator.FullName == perpetratorTemp.FullName)
                    {
                        isMatching = true;
                    }
                }
                return isMatching;
            }
        }

        class Perpetrator
        {
            internal string FullName { get; private set; }
            internal int Height { get; private set; }
            internal int Weight { get; private set; }
            internal string Nationality { get; private set; }
            internal string Status { get; private set; }

            private string[] _perpetratorFullNameBase = { "Нестер Евгения Ильинична", "Самиров Леонид Егорович", "Рязанцев Андрей Александрович", "Фунтов Юрий Геннадьевич", "Ивойлова Ксения Марселевна", "Шестунов Алексей Романович", "Ефанов Николай Алексеевич", "Петухина Алена Никитовна", "Качковский Вадим Васильевич", "Тунеева Маргарита Вадимовна", "Точилкина Анжелика Григорьевна", "Батраков Никита Павлович", "Вязмитинова Галина Яновна", "Индейкина Оксана Романовна", "Колосюк Руслан Янович", "Четков Михаил Ильич", "Хорошилова Надежда Кирилловна", "Кадулин Павел Тимурович", "Якименко Вероника Рамилевна", "Валиулин Дмитрий Данилович", "Тельпугова Евгения Артемовна", "Биушкина Татьяна Олеговна", "Славутинский Николай Игоревич", "Давыдов Александр Петрович", "Туаева Вероника Максимовна", "Мутовкина Ирина Васильевна", "Тактаров Эдуард Ринатович", "Златовратский Борис Павлович", "Недодаева Полина Аркадьевна", "Спиридонов Роман Борисович", "Лоринова Людмила Тимуровна", "Ряхин Марат Русланович", "Юльева Екатерина Ивановна", "Шуйгин Олег Максимович", "Проклов Глеб Валентинович", "Майданов Тимофей Алексеевич", "Славянинов Артур Маратович", "Таюпова Оксана Робертовна", "Коноплич Маргарита Андреевна", "Дратцева Римма Денисовна", "Гречановская Тамара Федоровна", "Петрищева Ирина Никитовна", "Шейхаметова Раиса Артуровна", "Сумцова Анжелика Геннадьевна", "Есиповская Татьяна Робертовна", "Свиногузова Кристина Ильдаровна", "Галанина Лидия Альбертовна", "Ледяева Жанна Константиновна", "Дудник Егор Радикович", "Гаянов Григорий Алексеевич" };
            private string[] _nationalityBase = { "Хобит", "Джигурдянин", "Ситх", "Астроидянин", "Марсианин", "Пеллинг" };
            private string[] _status = { "Заключенный", "Освобожден", "Не судим" };

            public Perpetrator()
            {
                GenerateFullName();
                GenerateHeight();
                GenerateWeight();
                GenerateNationality();
                GeneratePrisoner();
            }

            internal string[] GetnationalityBase()
            {
                string[] nationalityBase = new string[_nationalityBase.Length];

                for (int i = 0; i < _nationalityBase.Length; i++)
                {
                    nationalityBase[i] = _nationalityBase[i];
                }
                return nationalityBase;
            }

            private void GenerateFullName()
            {
                Random random = new Random();
                int minimumRandom = 0;
                int maximumRandom = _perpetratorFullNameBase.Length;

                FullName = _perpetratorFullNameBase[random.Next(minimumRandom, maximumRandom)];
            }

            private void GenerateHeight()
            {
                Random random = new Random();
                int minimumHeight = 165;
                int maximumHeight = 205;
                Height = random.Next(minimumHeight, maximumHeight);
            }

            private void GenerateWeight()
            {
                Random random = new Random();
                int minimumWeight = 65;
                int maximumWeight = 130;
                Weight = random.Next(minimumWeight, maximumWeight);
            }

            private void GenerateNationality()
            {
                Random random = new Random();
                int minimumRandom = 0;
                int maximumRandom = _nationalityBase.Length;
                Nationality = _nationalityBase[random.Next(minimumRandom, maximumRandom)];
            }

            private void GeneratePrisoner()
            {
                Random random = new Random();
                int minimumRandom = 0;
                int maximumRandom = _status.Length;
                Status = _status[random.Next(minimumRandom, maximumRandom)];
            }
        }
    }
}
