
using System;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;

namespace lab7
{
    internal class Program
    {
        static Tree tree = new();
        static List<Regex> types = [
            new(@"(\s*аквариумны(е|х|м))", RegexOptions.IgnoreCase),
            new(@"(\s*террариумны(е|х|м))", RegexOptions.IgnoreCase),
            new(@"(\s*птиц(ы|ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*млекопитающи(е|х|м))", RegexOptions.IgnoreCase)
        ];
        static List<Regex> species = [
            new(@"(\s*(Рыб(ы|ам)?))", RegexOptions.IgnoreCase),
            new(@"(\s*Рак(и|ов|ам))", RegexOptions.IgnoreCase),
            new(@"(\s*Паук(и|ов|ам))", RegexOptions.IgnoreCase),
            new(@"(\s*Зме(и|й|ям))", RegexOptions.IgnoreCase),
            new(@"(\s*Ящер(ы|ов|иц|ам|ицам))", RegexOptions.IgnoreCase),
            new(@"(\s*Попуга(и|ев|ям))", RegexOptions.IgnoreCase),
            new(@"(\s*Кош(ки|ек|кам))", RegexOptions.IgnoreCase),
            new(@"(\s*Собак(и|ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Кролик(и|ов|ам))", RegexOptions.IgnoreCase)
        ];
        static List<Regex> breeds = [
            new(@"(\s*(Рыба(м)?-клоун(ам)?))", RegexOptions.IgnoreCase),
            new(@"(\s*Моллинезия(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Гуппи)", RegexOptions.IgnoreCase),
            new(@"(\s*Рыба(м)?-попуга(й|ям))", RegexOptions.IgnoreCase),
            new(@"(\s*Креветка(м)? Блю Дрим)", RegexOptions.IgnoreCase),
            new(@"(\s*Рак(ам)? Ябби)", RegexOptions.IgnoreCase),
            new(@"(\s*Американски(й|м) Домашни(й|м) Паук(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Черн(ая|ым) Вдова(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Полоз(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Питон(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Удав(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Василиск(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Плащеносн(ая|ым) ящерица(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Хамелеон(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Игуана(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Волнисты(й|м) попуга(й|ям))", RegexOptions.IgnoreCase),
            new(@"(\s*Неразлучник(и|ам))", RegexOptions.IgnoreCase),
            new(@"(\s*Американск(ая|им) Короткошёрстн(ая|ым) кошка(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Бирманск(ая|им) кошка(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Британск(ая|им) короткошёрстн(ая|им) кошка(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Персидск(ая|им) кошка(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Сиамск(ая|им) кошка(м)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Джек-рассел-терьер(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Мопс(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Корги)", RegexOptions.IgnoreCase),
            new(@"(\s*Алаба(й|ям))", RegexOptions.IgnoreCase),
            new(@"(\s*Цветн(ой|ым) кролик(ам)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Шиншилл(ам)?)", RegexOptions.IgnoreCase)];

        static List<Regex> dataNamesRegs = [
            new(@"(\s*(Рыб(а|у)-клоун(а)?))", RegexOptions.IgnoreCase),
            new(@"(\s*Моллинези(я|ю)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Гуппи)", RegexOptions.IgnoreCase),
            new(@"(\s*Рыб(а|у)-попуга(й|я))", RegexOptions.IgnoreCase),
            new(@"(\s*Креветк(а|у) Блю Дрим)", RegexOptions.IgnoreCase),
            new(@"(\s*Рак(а)? Ябби)", RegexOptions.IgnoreCase),
            new(@"(\s*Американск(ий|ого) Домашн(ий|его) паук(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Черн(ая|ую) вдов(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Полоз(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Питон(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Удав(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Василиск(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Плащеносн(ая|ую) ящериц(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Хамелеон(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Игуан(а|у)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Волнист(ый|ого) попуга(й|я))", RegexOptions.IgnoreCase),
            new(@"(\s*Неразлучник(а)?1)", RegexOptions.IgnoreCase),
            new(@"(\s*Неразлучник(а)?2)", RegexOptions.IgnoreCase),
            new(@"(\s*Американск(ая|ую) Короткошёрстн(ая|ую) кошк(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Бирманск(ая|ую) кошк(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Британск(ая|ую) короткошёрстн(ая|ую) кошк(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Персидск(ая|ую) кошк(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Сиамск(ая|ую) кошк(а|у))", RegexOptions.IgnoreCase),
            new(@"(\s*Джек-рассел-терьер(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Мопс(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Корги)", RegexOptions.IgnoreCase),
            new(@"(\s*Алаба(й|я))", RegexOptions.IgnoreCase),
            new(@"(\s*Цветн(ой|ого) кролик(а)?)", RegexOptions.IgnoreCase),
            new(@"(\s*Шиншилл(а|у))", RegexOptions.IgnoreCase)];
        static Regex TSB = new(@"(\s*(типы|виды|породы))", RegexOptions.IgnoreCase);
        static Regex SB = new(@"(\s*(виды|породы))", RegexOptions.IgnoreCase);
        static Regex objects = new(@"(\s*(домашн(их|ие|ee))?\s*(животн(ых|ые|ое)|питом(е)?ц(а|ев|ы)?))", RegexOptions.IgnoreCase);
        static Regex pointer = new(@"(из этих|из них)", RegexOptions.IgnoreCase);
        static Filter Filtration = new();
        static void Main(string[] args)
        {
            Regex class1Color1 = new($"^(Какого цвета|Каких цветов) бывают ({objects})?", RegexOptions.IgnoreCase);
            Regex class1Color2 = new($"^(Какие бывают|Перечисли) (все )?цвета({objects})?", RegexOptions.IgnoreCase);
            Regex class1Properties = new($"^(Какие бывают|Перечисли) признаки( у)?({objects}?)", RegexOptions.IgnoreCase);
            Regex class1WhatIs = new("^Что такое " + objects, RegexOptions.IgnoreCase);
            Regex class1Vaccine = new("^Что (значит|такое) вакцинация", RegexOptions.IgnoreCase);
            Regex class1size = new($"^Что (значит|такое) размер({objects})*", RegexOptions.IgnoreCase);
            Regex class1sizeMeas = new($"^В чем измеряется размер({objects})*", RegexOptions.IgnoreCase);
            Regex class1Cond = new($"^(Какие бывают|Перечисли) условия содержания({objects})?", RegexOptions.IgnoreCase);
            Regex class1TSB = new($"^(Какие бывают|Перечисли) {TSB}({objects})?", RegexOptions.IgnoreCase);
            Regex class1SB = new($"^(Какие бывают|Перечисли){SB} (у)?({string.Join("|",types)})({objects})?", RegexOptions.IgnoreCase);
            Regex class1B = new($"^(Какие бывают|Перечисли) породы (у)?({string.Join("|",species)})({objects})?", RegexOptions.IgnoreCase);

            Regex class2cond = new(@$"{pointer}?\s?(выбери|покажи|перечисли|какие|для каких)\s*(мне)?\s*(все(х)?)?{objects}\s*(у)?\s*(которых|для которых|которым|которые)?\s*(?<cond>.+)",RegexOptions.IgnoreCase);
            Regex class2Extr = new(@$"{pointer}?\s?(покажи|выбери|какое){objects}\s*(которое)? (?<cond>самое.*)", RegexOptions.IgnoreCase);
            Regex singleCondRegex = new(@"(?<rest>.*) (?<op>(?<andor>и|но|или)\s*(?<not>не)?) (?<filter>.*)", RegexOptions.IgnoreCase);
            Regex lastCondRegex = new(@"(?<not>не)?(?<filter>[\w\s]*)", RegexOptions.IgnoreCase);

            Regex class3LikeSimple = new(@$"мне\s*(?<not>не)?\s*(нрав(я|и)тся)(?<list>[\w\s,]+)(\. | )(порекомендуй|посоветуй)\sчто-нибудь\s*({pointer})*", RegexOptions.IgnoreCase);
            Regex class3LikeHard = new(@$"мне\s*(нрав(я|и)тся)(?<l>[\w\s,]*) и не (нрав(я|и)тся)(?<dl>[\w\s,]*).*(порекомендуй|посоветуй)\sчто-нибудь\s*{pointer}*", RegexOptions.IgnoreCase);
            Regex class3similar = new(@$"хочу завести\s*{pointer}\s*(что-нибудь|како(е|го)-нибудь\s*((домашн(их|ие|ee))?\s*(животн(ых|ые|ое)|питом(е)?ц(а|ев|ы)?))) (?<rec>(?<not>не)?\s*похоже(е|го) на (?<recList>.*))", RegexOptions.IgnoreCase);
            Regex class3better = new(@"мне\s*(?<not>не)?\s*нравится\s*(?<likePet> .*)( |\.)(кого|что) (мне)?\s*лучше взять (?<pet1>.*) или (?<pet2>.*)", RegexOptions.IgnoreCase);

            Regex class4Diff = new(@"чем отличается (?<pet1>.*) от (?<pet2>.*)", RegexOptions.IgnoreCase);
            Regex class4Sim = new(@"в чем (отличие|сходство|разница между) (?<pet1>.*) и (?<pet2>.*)",RegexOptions.IgnoreCase);
            Regex class23 = new(@"мне нравится (?<pet>[\w\s]*)\. (посоветуй|порекомендуй|покажи)\s*(что-нибудь)?\s*(?<not>не)?\s*похожее\s*(на него)?.*но чтобы\s*(?<cond>.+)", RegexOptions.IgnoreCase);
            InitData();
            СходствоЖивотных RecommendSystem = new(tree);
            var AnimalsList = RecommendSystem.animals;
            var inputString = "";
            do
            {
                inputString = Console.ReadLine() ?? "";
                if (!pointer.IsMatch(inputString))
                {
                    Filtration.ClearPredicates();
                    RecommendSystem = new(AnimalsList);
                }
                if (string.IsNullOrEmpty(inputString)) break;
                else if (class1Color1.IsMatch(inputString) || class1Color2.IsMatch(inputString))
                {
                    Console.WriteLine("бывают цвета {0}", string.Join("; ", Filtration.Color));
                }
                else if (class1Properties.IsMatch(inputString))
                {
                    Console.WriteLine("Вывожу абстрактное животное со всем списком признаков");
                    PrintPet(new Pet());
                }
                else if (class1WhatIs.IsMatch(inputString))
                {
                    Console.WriteLine("Животные, которые были одомашнены человеком разумным и которых он содержит, предоставляя им кров и пищу. Они приносят ему пользу либо как источник материальных благ и услуг, либо как животные-компаньоны, скрашивающие его досуг. ");
                }
                else if (class1Vaccine.IsMatch(inputString))
                {
                    Console.WriteLine("Вакцинация животных - распространенная процедура, наиболее частой является прививка от бешенства.");
                }
                else if (class1Cond.IsMatch(inputString))
                {
                    Console.WriteLine("Условия содержания бывают: \n{0}", string.Join("\n", Filtration.Condition));
                }
                else if (class1size.IsMatch(inputString))
                {
                    Console.WriteLine("Размер животного это высота для млекопитающих и длина для остальных");
                }
                else if (class1sizeMeas.IsMatch(inputString))
                {
                    Console.WriteLine("Размер измеряется в сантиметрах");
                }
                else if (class1B.IsMatch(inputString))
                {
                    Regex? specie = null;
                    foreach (var r in species)
                    {
                        if (r.IsMatch(inputString))
                        {
                            specie = r;
                            break;
                        }
                    }
                    if (specie == null)
                    {
                        continue;
                    }
                    var speciesAnim = AnimalsList.Select(a => a.Specie).ToList();
                    var s = speciesAnim.FindAll(s => specie.IsMatch(s.Name));
                    Console.WriteLine("Вывести породы для вида {0}\n {1} ", s[0].Name, string.Join("\n", s.Select(b => b.Breeds[0].Name)));
                }
                else if (class1SB.IsMatch(inputString))
                {
                    Regex? type = null;
                    foreach (var r in types)
                    {
                        if (r.IsMatch(inputString))
                        {
                            type = r;
                            break;
                        }
                    }
                    if (type == null)
                    {
                        continue;
                    }
                    if (SB.Match(inputString).Value.Trim().ToLower() == "виды")
                    {
                        var species = AnimalsList.Where(p => type.IsMatch(p.Type.Name)).Select(p => p.Specie.Name).ToHashSet();
                        Console.WriteLine("Виды животных для типа {1}\n {0}", string.Join("\n", species), AnimalsList.Find(p => type.IsMatch(p.Type.Name)).Type.Name);
                    }
                    else if (SB.Match(inputString).Value.Trim().ToLower() == "породы")
                    {
                        var b = AnimalsList.Where(p => type.IsMatch(p.Type.Name)).Select(p => p.Breed.Name).ToHashSet();
                        Console.WriteLine("Породы животных для типа {1}\n {0}", string.Join("\n", b), AnimalsList.Find(p => type.IsMatch(p.Type.Name)).Type.Name);
                    }
                }
                else if (class1TSB.IsMatch(inputString))
                {
                    if (TSB.Match(inputString).Value.Trim().ToLower() == "типы")
                    {
                        Console.WriteLine("Типы животных\n {0}", string.Join("\n ", tree.Types.Select(t => t.Name)));
                    }
                    else if (TSB.Match(inputString).Value.Trim().ToLower() == "виды")
                    {
                        Console.WriteLine("Виды животных\n {0}", string.Join("\n ", AnimalsList.Select(p => p.Specie.Name).ToHashSet()));
                    }
                    else if (TSB.Match(inputString).Value.Trim().ToLower() == "породы")
                    {
                        Console.WriteLine("Породы животных\n {0}", string.Join("\n", AnimalsList.Select(p => p.Breed.Name).ToHashSet()));
                    }
                }
                else if (class2Extr.IsMatch(inputString))
                {
                    var cond = class2Extr.Match(inputString).Groups.GetValueOrDefault("cond")!.Value;
                    var filteredAnim = RecommendSystem.animals.Where((pet) =>
                    {
                        bool res = true;
                        foreach (var filter in Filtration.Predicates)
                        {
                            res = filter(pet, res);
                        }
                        return res;
                    });
                    var ordered = filteredAnim.OrderByDescending(pet => pet.Size);
                    if (cond.Contains("больш") || cond.Contains("больше всех"))
                        ordered = filteredAnim.OrderByDescending(pet => pet.Size);
                    if (cond.Contains("маленьк") || cond.Contains("меньше всех"))
                        ordered = filteredAnim.OrderBy(pet => pet.Size);
                    if (cond.Contains("стар") || cond.Contains("старше всех"))
                        ordered = filteredAnim.OrderByDescending(pet => pet.Age);
                    if (cond.Contains("молод") || cond.Contains("младше всех"))
                        ordered = filteredAnim.OrderBy(pet => pet.Age);
                    if (cond.Contains("живет дольше всех"))
                        ordered = filteredAnim.OrderByDescending(pet => pet.AvgAge);
                    if (cond.Contains("живет меньше всех"))
                        ordered = filteredAnim.OrderBy(pet => pet.AvgAge);

                    Console.WriteLine($"{(ordered.FirstOrDefault() ?? new Pet()).Name}");
                }
                else if (class2cond.IsMatch(inputString))
                {
                    var cond = class2cond.Match(inputString).Groups.GetValueOrDefault("cond");

                    Console.WriteLine("Целое условие: {0}", cond!.Value);
                    var andorValue = "и";
                    while (singleCondRegex.IsMatch(cond.Value))
                    {
                        var singleCond = singleCondRegex.Match(cond.Value);
                        cond = singleCond.Groups.GetValueOrDefault("rest");

                        var andor = singleCond.Groups.GetValueOrDefault("andor");
                        var not = singleCond.Groups.GetValueOrDefault("not");
                        var filter = singleCond.Groups.GetValueOrDefault("filter");
                        ConditionFilter(Filtration, andorValue, not, filter);
                        ColorFilter(Filtration, andorValue, not, filter);
                        TypeFilter(Filtration, andorValue, not, filter);
                        SpecieFilter(Filtration, andorValue, not, filter);
                        BreedFilter(Filtration, andorValue, not, filter);
                        SizeFilter(Filtration, andorValue, not, filter);
                        AgeFilter(Filtration, andorValue, not, filter);
                        AvgAgeFilter(Filtration, andorValue, not, filter);
                        ActiveFilter(Filtration, andorValue, not, filter);
                        VaccineFilter(Filtration, andorValue, not, filter);
                        andorValue = andor!.Value.Trim();
                    }

                    var lastCond = lastCondRegex.Match(cond.Value);
                    var lastNot = lastCond.Groups.GetValueOrDefault("not");
                    var lastFilter = lastCond.Groups.GetValueOrDefault("filter");
                    ConditionFilter(Filtration, andorValue, lastNot, lastFilter);
                    ColorFilter(Filtration, andorValue, lastNot, lastFilter);
                    TypeFilter(Filtration, andorValue, lastNot, lastFilter);
                    SpecieFilter(Filtration, andorValue, lastNot, lastFilter);
                    BreedFilter(Filtration, andorValue, lastNot, lastFilter);
                    SizeFilter(Filtration, andorValue, lastNot, lastFilter);
                    AgeFilter(Filtration, andorValue, lastNot, lastFilter);
                    AvgAgeFilter(Filtration, andorValue, lastNot, lastFilter);
                    ActiveFilter(Filtration, andorValue, lastNot, lastFilter);
                    VaccineFilter(Filtration, andorValue, lastNot, lastFilter);
                    var filteredAnim = RecommendSystem.animals.Where((pet) =>
                    {
                        bool res = true;
                        foreach (var filter in Filtration.Predicates)
                        {
                            res = filter(pet, res);
                        }
                        return res;
                    });
                    var filterList = filteredAnim.ToList();
                    RecommendSystem = new(filterList);
                    foreach (var pet in filterList)
                    {
                        Console.WriteLine($"{pet.Name}");
                    }
                }
                else if (class23.IsMatch(inputString))
                {
                    var cond = class23.Match(inputString).Groups.GetValueOrDefault("cond");

                    Console.WriteLine("Целое условие: {0}", cond!.Value);
                    var andorValue = "и";
                    while (singleCondRegex.IsMatch(cond.Value))
                    {
                        var singleCond = singleCondRegex.Match(cond.Value);
                        cond = singleCond.Groups.GetValueOrDefault("rest");

                        var andor = singleCond.Groups.GetValueOrDefault("andor");
                        var not = singleCond.Groups.GetValueOrDefault("not");
                        var filter = singleCond.Groups.GetValueOrDefault("filter");
                        ConditionFilter(Filtration, andorValue, not, filter);
                        ColorFilter(Filtration, andorValue, not, filter);
                        TypeFilter(Filtration, andorValue, not, filter);
                        SpecieFilter(Filtration, andorValue, not, filter);
                        BreedFilter(Filtration, andorValue, not, filter);
                        SizeFilter(Filtration, andorValue, not, filter);
                        AgeFilter(Filtration, andorValue, not, filter);
                        AvgAgeFilter(Filtration, andorValue, not, filter);
                        ActiveFilter(Filtration, andorValue, not, filter);
                        VaccineFilter(Filtration, andorValue, not, filter);
                        andorValue = andor!.Value.Trim();
                    }

                    var lastCond = lastCondRegex.Match(cond.Value);
                    var lastNot = lastCond.Groups.GetValueOrDefault("not");
                    var lastFilter = lastCond.Groups.GetValueOrDefault("filter");
                    ConditionFilter(Filtration, andorValue, lastNot, lastFilter);
                    ColorFilter(Filtration, andorValue, lastNot, lastFilter);
                    TypeFilter(Filtration, andorValue, lastNot, lastFilter);
                    SpecieFilter(Filtration, andorValue, lastNot, lastFilter);
                    BreedFilter(Filtration, andorValue, lastNot, lastFilter);
                    SizeFilter(Filtration, andorValue, lastNot, lastFilter);
                    AgeFilter(Filtration, andorValue, lastNot, lastFilter);
                    AvgAgeFilter(Filtration, andorValue, lastNot, lastFilter);
                    ActiveFilter(Filtration, andorValue, lastNot, lastFilter);
                    VaccineFilter(Filtration, andorValue, lastNot, lastFilter);

                    var likePet = class23.Match(inputString).Groups.GetValueOrDefault("pet")!.Value.Trim().ToLower();
                    var notLike = class23.Match(inputString).Groups.GetValueOrDefault("not")!.Value.Trim().ToLower();
                    Regex? likePetR = null;
                    foreach (var regName in dataNamesRegs)
                    {
                        if (regName.IsMatch(likePet))
                            likePetR = regName;
                    }
                    if (likePetR == null)
                    {
                        Console.WriteLine("Неправильно введено имя животного");
                        continue;
                    }
                    var notCond = !string.IsNullOrEmpty(notLike.Trim().ToLower());
                    foreach (var pet in RecommendSystem.animals)
                    {
                        pet.Like = "0";
                    }
                    RecommendSystem.animals.Find(pet => likePetR.IsMatch(pet.Name))!.Like = notCond ? "-" : "+";

                    var recommended = RecommendSystem.Recommend();

                    var filteredAnim = recommended.Where((pet) =>
                    {
                        bool res = true;
                        foreach (var filter in Filtration.Predicates)
                        {
                            res = filter(pet, res);
                        }
                        return res;
                    });
                    RecommendSystem = new(filteredAnim.ToList());

                    foreach (var pet in filteredAnim)
                    {
                        Console.WriteLine($"{pet.Name}");
                    }
                }
                else if (class3better.IsMatch(inputString))
                {
                    var likePet = class3better.Match(inputString).Groups.GetValueOrDefault("likePet")!.Value.Trim().ToLower();
                    var pet1 = class3better.Match(inputString).Groups.GetValueOrDefault("pet1")!.Value.Trim().ToLower();
                    var pet2 = class3better.Match(inputString).Groups.GetValueOrDefault("pet2")!.Value.Trim().ToLower();
                    var not = class3better.Match(inputString).Groups.GetValueOrDefault("not")!.Value.Trim().ToLower();
                    Regex? likePetR = null, pet1R = null, pet2R = null;
                    foreach (var regName in dataNamesRegs)
                    {
                        if (regName.IsMatch(likePet))
                            likePetR = regName;
                        if (regName.IsMatch(pet1))
                            pet1R = regName;
                        if (regName.IsMatch(pet2))
                            pet2R = regName;
                    }
                    if (likePetR == null || pet1R == null || pet2R == null)
                    {
                        Console.WriteLine("Неправильно введено имя животного");
                        continue;
                    }
                    var notCond = !string.IsNullOrEmpty(not.Trim().ToLower());
                    foreach (var pet in RecommendSystem.animals)
                    {
                        pet.Like = "0";
                    }
                    RecommendSystem.animals.Find(pet => likePetR.IsMatch(pet.Name))!.Like = notCond ? "-" : "+";
                    var recommended = RecommendSystem.Recommend();
                    var pet1I = recommended.FindIndex(pet => pet1R.IsMatch(pet.Name));
                    var pet2I = recommended.FindIndex(pet => pet2R.IsMatch(pet.Name));
                    if (pet1I < 0 && pet2I < 0)
                        Console.WriteLine("На основе данных не возможно сделать предположение");
                    else if (pet1I < 0)
                        Console.WriteLine("Лучше брать {0}", pet2);
                    else if (pet2I < 0)
                        Console.WriteLine("Лучше брать {0}", pet1);
                    else
                        Console.WriteLine("Лучше брать {0}", pet1I < pet2I ? pet1 : pet2);
                }
                else if (class3similar.IsMatch(inputString))
                {
                    var recList = class3similar.Match(inputString).Groups.GetValueOrDefault("recList")!.Value.Trim().ToLower();
                    var not = class3similar.Match(inputString).Groups.GetValueOrDefault("not")!.Value.Trim().ToLower();
                    var notCond = !string.IsNullOrEmpty(not.Trim().ToLower());
                    Console.WriteLine("{1}лайки: {0}", recList, notCond ? "диз" : "");
                    Regex listReg = new(@"((?<tail>.*)\sили\s)?(?<head>.*)");
                    foreach (var pet in RecommendSystem.animals)
                    {
                        pet.Like = "0";
                    }
                    string head;
                    do
                    {
                        head = listReg.Match(recList).Groups.GetValueOrDefault("head")!.Value.Trim().ToLower();
                        recList = listReg.Match(recList).Groups.GetValueOrDefault("tail")!.Value.Trim().ToLower();
                        Regex? petReg = null;
                        foreach (var regName in dataNamesRegs)
                        {
                            if (regName.IsMatch(head))
                                petReg = regName;
                        }
                        if (petReg == null)
                        {
                            continue;
                        }
                        RecommendSystem.animals.Find(pet => petReg.IsMatch(pet.Name))!.Like = notCond ? "-" : "+";
                    }
                    while (!string.IsNullOrEmpty(head));
                    var recommended = RecommendSystem.Recommend();
                    foreach (var pet in recommended)
                    {
                        Console.WriteLine($"{pet.Name}");
                    }
                }
                else if (class3LikeHard.IsMatch(inputString))
                {
                    var dl = class3LikeHard.Match(inputString).Groups.GetValueOrDefault("dl")!.Value.Trim().ToLower();
                    var l = class3LikeHard.Match(inputString).Groups.GetValueOrDefault("l")!.Value.Trim().ToLower();
                    Console.WriteLine("дизлайки: {0}; лайки: {1}", dl, l);
                    Regex listReg = new(@"((?<tail>.*),)?(?<head>.*)");
                    foreach (var pet in RecommendSystem.animals)
                    {
                        pet.Like = "0";
                    }
                    string head;
                    do
                    {
                        head = listReg.Match(dl).Groups.GetValueOrDefault("head")!.Value.Trim().ToLower();
                        dl = listReg.Match(dl).Groups.GetValueOrDefault("tail")!.Value.Trim().ToLower();
                        Regex? petReg = null;
                        foreach (var regName in dataNamesRegs)
                        {
                            if (regName.IsMatch(head))
                                petReg = regName;
                        }
                        if (petReg == null)
                        {
                            continue;
                        }
                        RecommendSystem.animals.Find(pet => petReg.IsMatch(pet.Name))!.Like = "-";
                    }
                    while (!string.IsNullOrEmpty(head));
                    do
                    {
                        head = listReg.Match(l).Groups.GetValueOrDefault("head")!.Value.Trim().ToLower();
                        l = listReg.Match(l).Groups.GetValueOrDefault("tail")!.Value.Trim().ToLower();
                        Regex? petReg = null;
                        foreach (var regName in dataNamesRegs)
                        {
                            if (regName.IsMatch(head))
                                petReg = regName;
                        }
                        if (petReg == null)
                        {
                            continue;
                        }
                        RecommendSystem.animals.Find(pet => petReg.IsMatch(pet.Name))!.Like = "+";
                    }
                    while (string.IsNullOrEmpty(head));
                    var recommended = RecommendSystem.Recommend();
                    RecommendSystem = new(recommended);
                    foreach (var pet in recommended)
                    {
                        Console.WriteLine($"{pet.Name}");
                    }
                }
                else if (class3LikeSimple.IsMatch(inputString))
                {
                    var list = class3LikeSimple.Match(inputString).Groups.GetValueOrDefault("list")!.Value.Trim().ToLower();
                    var not = class3LikeSimple.Match(inputString).Groups.GetValueOrDefault("not")!.Value.Trim().ToLower();
                    var notCond = !string.IsNullOrEmpty(not.Trim().ToLower());
                    Console.WriteLine("{1}лайки: {0}", list, notCond ? "диз" : "");
                    Regex listReg = new(@"((?<tail>.*),)?(?<head>.*)");
                    foreach (var pet in RecommendSystem.animals)
                    {
                        pet.Like = "0";
                    }
                    string head;
                    do
                    {
                        head = listReg.Match(list).Groups.GetValueOrDefault("head")!.Value.Trim().ToLower();
                        list = listReg.Match(list).Groups.GetValueOrDefault("tail")!.Value.Trim().ToLower();
                        Regex? petReg = null;
                        foreach (var regName in dataNamesRegs)
                        {
                            if (regName.IsMatch(head))
                                petReg = regName;
                        }
                        if (petReg == null)
                        {
                            continue;
                        }
                        RecommendSystem.animals.Find(pet => petReg.IsMatch(pet.Name))!.Like = notCond ? "-" : "+";
                    }
                    while (string.IsNullOrEmpty(head));
                    var recommended = RecommendSystem.Recommend();
                    RecommendSystem = new(recommended);
                    foreach (var pet in recommended)
                    {
                        Console.WriteLine($"{pet.Name}");
                    }
                }
                else if (class4Diff.IsMatch(inputString) || class4Sim.IsMatch(inputString))
                {
                    string pet1, pet2;
                    if (class4Diff.IsMatch(inputString))
                    {
                        pet1 = class4Diff.Match(inputString).Groups.GetValueOrDefault("pet1")!.Value.Trim().ToLower();
                        pet2 = class4Diff.Match(inputString).Groups.GetValueOrDefault("pet2")!.Value.Trim().ToLower();
                    }
                    else
                    {
                        pet1 = class4Sim.Match(inputString).Groups.GetValueOrDefault("pet1")!.Value.Trim().ToLower();
                        pet2 = class4Sim.Match(inputString).Groups.GetValueOrDefault("pet2")!.Value.Trim().ToLower();
                    }

                    Regex? pet1R = null, pet2R = null;
                    foreach (var regName in dataNamesRegs)
                    {

                        if (regName.IsMatch(pet1))
                            pet1R = regName;
                        if (regName.IsMatch(pet2))
                            pet2R = regName;
                    }
                    if (pet1R == null || pet2R == null)
                    {
                        Console.WriteLine("Неправильно введено имя животного");
                        continue;
                    }
                    var Pet1 = AnimalsList.Find(pet => pet1R.IsMatch(pet.Name)) ?? new Pet();
                    var Pet2 = AnimalsList.Find(pet => pet2R.IsMatch(pet.Name)) ?? new Pet();
                    PrintPets(Pet1, Pet2);
                }
                else
                {
                    Console.WriteLine("Вопрос не распознан");
                }
                Console.WriteLine();
            } while (!string.IsNullOrEmpty(inputString));
        }


        static void PrintPets(Pet pet1, Pet pet2)
        {
            Console.Write("Имя: {0}\t", pet1.Name);
            Console.WriteLine("{0}", pet2.Name);
            if (pet1.Vacinied != pet2.Vacinied)
            {
                Console.Write("Вакцинация: {0}\t", pet1.Vacinied ? "+" : "-");
                Console.WriteLine("{0}", pet2.Vacinied ? "+" : "-");
            }
            if (pet1.Age != pet2.Age)
            {
                Console.Write("Возраст: {0}\t", pet1.Age.ToString());
                Console.WriteLine("{0}", pet2.Age.ToString());
            }
            if (pet1.AvgAge != pet2.AvgAge)
            {
                Console.Write("Продолжительность жизни: {0}\t", pet1.AvgAge.ToString());
                Console.WriteLine("{0}", pet2.AvgAge.ToString());
            }
            if (pet1.Active != pet2.Active)
            {
                Console.Write("Активный: {0}\t", pet1.Active ? "+" : "-");
                Console.WriteLine("{0}", pet2.Active ? "+" : "-");
            }

            var color1 = "";
            for (ulong i = pet1.Color, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    color1 += Filtration.Color[(int)k] + " ";
                }
            }
            var color2 = "";
            for (ulong i = pet2.Color, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    color2 += Filtration.Color[(int)k] + " ";
                }
            }
            if (pet1.Color != pet2.Color)
            {
                Console.Write("Цвет: {0};\t", color1);
                Console.WriteLine("{0}", color2);
            }
            List<string> lines1 = [];
            for (ulong i = pet1.Conditions, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    lines1.Add(Filtration.Condition[(int)k]);
                }
            }
            List<string> lines2 = [];
            for (ulong i = pet2.Conditions, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    lines2.Add(Filtration.Condition[(int)k]);
                }
            }
            if (pet1.Conditions != pet2.Conditions)
            {
                Console.Write("Условия содержания: {0};\t", string.Join(", ", lines1));
                Console.WriteLine("{0}", string.Join(", ", lines2));
            }
            if (pet1.Type.Name != pet2.Type.Name)
            {
                Console.Write("Тип: {0}\t", pet1.Type.Name);
                Console.WriteLine("{0}", pet2.Type.Name);
            }
            if (pet1.Specie.Name != pet2.Specie.Name)
            {
                Console.Write("Вид: {0}\t", pet1.Specie.Name);
                Console.WriteLine("{0}", pet2.Specie.Name);
            }
            if (pet1.Breed.Name != pet2.Breed.Name)
            {
                Console.Write("Порода: {0}\t", pet1.Breed.Name);
                Console.WriteLine("{0}", pet2.Breed.Name);
            }
            var size1 = pet1.Size.ToString();
            var size2 = pet2.Size.ToString();

            var measure1 = "";

            if (pet1.Type.Name == "Млекопитающие")
            {
                measure1 = "Высота, см";
            }
            else
            {
                measure1 = "Длина, см";
            }
            var measure2 = "";
            if (pet2.Type.Name == "Млекопитающие")
            {
                measure2 = "Высота, см";
            }
            else
            {
                measure2 = "Длина, см";
            }
            if (size1 != size2 || measure1 != measure2)
            {
                Console.Write("Размер: {0} {1}\t", size1, measure1);
                Console.WriteLine("{0} {1}", size2, measure2);
            }
        }

        static void PrintPet(Pet pet)
        {
            Console.WriteLine("Имя: {0}", pet.Name);
            Console.WriteLine("Вакцинация: {0}", pet.Vacinied ? "+" : "-");
            Console.WriteLine("Возраст: {0}", pet.Age.ToString());
            Console.WriteLine("Продолжительность жизни: {0}", pet.AvgAge.ToString());
            Console.WriteLine("Активный: {0}", pet.Active ? "+" : "-");
            var color = "";
            for (ulong i = pet.Color, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    color += Filtration.Color[(int)k] + " ";
                }
            }
            Console.WriteLine("Цвет: {0}", color);
            List<string> lines = [];
            for (ulong i = pet.Conditions, k = 0; i > 0; i >>= 1, k++)
            {
                if ((i & 1) == 1)
                {
                    lines.Add(Filtration.Condition[(int)k]);
                }
            }
            Console.WriteLine("Условия содержания: {0}", string.Join(' ', lines));

            Console.WriteLine("Тип: {0}", pet.Type.Name);
            Console.WriteLine("Вид: {0}", pet.Specie.Name);
            Console.WriteLine("Порода: {0}", pet.Breed.Name);
            var size = pet.Size.ToString();
            var measure = "";

            if (pet.Type.Name == "Млекопитающие")
            {
                measure = "Высота, см";
            }
            else
            {
                measure = "Длина, см";
            }
            Console.WriteLine("Размер: {0} {1}", size, measure);
        }

        static void ConditionFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            var conditionI = filtration.Condition.IndexOf(filter!.Value.Trim().ToLower());
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());
            if (conditionI > -1)
            {
                if (op == "и" || op == "но")
                    filtration.SetConditionFilter(false, notCond, conditionI);
                else
                    filtration.SetConditionFilter(true, notCond, conditionI);
            }
        }

        static void ColorFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            var colorI = filtration.Color.IndexOf(filter!.Value.Trim().ToLower());
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());
            if (colorI > -1)
            {
                if (op == "и" || op == "но")
                    filtration.SetColorFilter(false, notCond, colorI);
                else
                    filtration.SetColorFilter(true, notCond, colorI);
            }
        }

        static Regex belongsRegex = new(@"(?<belong>относятся к)\s*(?<filter>[\w\s]*)");

        static void TypeFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!belongsRegex.IsMatch(filter!.Value))
                return;
            var type = belongsRegex.Match(filter!.Value).Groups.GetValueOrDefault("filter")!.Value;
            Regex? typeR = null;
            foreach (var r in types)
            {
                if (r.IsMatch(type))
                {
                    typeR = r;
                    break;
                }
            }
            if (typeR == null)
            {
                return;
            }
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());

            if (op == "и" || op == "но")
                filtration.SetTypeFilter(false, notCond, typeR);
            else
                filtration.SetTypeFilter(true, notCond, typeR);

        }

        static void SpecieFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!belongsRegex.IsMatch(filter!.Value))
                return;
            var specie = belongsRegex.Match(filter!.Value).Groups.GetValueOrDefault("filter")!.Value;
            Regex? specieR = null;
            foreach (var r in species)
            {
                if (r.IsMatch(specie))
                {
                    specieR = r;
                    break;
                }
            }
            if (specieR == null)
            {
                return;
            }
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());

            if (op == "и" || op == "но")
                filtration.SetSpecieFilter(false, notCond, specieR);
            else
                filtration.SetSpecieFilter(true, notCond, specieR);
        }
        static void BreedFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!belongsRegex.IsMatch(filter!.Value))
                return;
            var breed = belongsRegex.Match(filter!.Value).Groups.GetValueOrDefault("filter")!.Value;
            Regex? breedR = null;
            foreach (var r in breeds)
            {
                if (r.IsMatch(breed))
                {
                    breedR = r;
                    break;
                }
            }
            if (breedR == null)
            {
                return;
            }
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());

            if (op == "и" || op == "но")
                filtration.SetBreedFilter(false, notCond, breedR);
            else
                filtration.SetBreedFilter(true, notCond, breedR);
        }

        static Regex numberRegex = new("(?<prop>размер|продолжительность жизни|возраст) (?<comparator>больше|меньше|не больше|не меньше|рав(ен|на)|не рав(ен|на)|>|<|>=|<=|=|!=) (?<num>\\d*,?\\d*)", RegexOptions.IgnoreCase);

        static void SizeFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!numberRegex.IsMatch(filter!.Value))
                return;
            var prop = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("prop")!.Value;
            var comparator = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("comparator")!.Value;
            if (!float.TryParse(numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("num")!.Value, out float num))
                return;
            if (prop != "размер")
                return;
            Predicate<float> comparatorPredicate = n => true;
            if (comparator == "больше" || comparator == ">")
                comparatorPredicate = n => n > num;
            else if (comparator == "меньше" || comparator == "<")
                comparatorPredicate = n => n < num;
            else if (comparator == "не больше" || comparator == "<=")
                comparatorPredicate = n => n <= num;
            else if (comparator == "не меньше" || comparator == ">=")
                comparatorPredicate = n => n >= num;
            else if (comparator == "не равно" || comparator == "!=")
                comparatorPredicate = n => n != num;
            else if (comparator.StartsWith("рав") || comparator == "=")
                comparatorPredicate = n => n == num;

            if (op == "и" || op == "но")
                filtration.SetSizeFilter(false, comparatorPredicate);
            else
                filtration.SetSizeFilter(true, comparatorPredicate);
        }

        static void AgeFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!numberRegex.IsMatch(filter!.Value))
                return;
            var prop = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("prop")!.Value;
            var comparator = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("comparator")!.Value;
            if (!float.TryParse(numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("num")!.Value, out float num))
                return;
            if (prop != "возраст")
                return;
            Predicate<float> comparatorPredicate = n => true;
            if (comparator == "больше" || comparator == ">")
                comparatorPredicate = n => n > num;
            else if (comparator == "меньше" || comparator == "<")
                comparatorPredicate = n => n < num;
            else if (comparator == "не больше" || comparator == "<=")
                comparatorPredicate = n => n <= num;
            else if (comparator == "не меньше" || comparator == ">=")
                comparatorPredicate = n => n >= num;
            else if (comparator == "не равно" || comparator == "!=")
                comparatorPredicate = n => n != num;
            else if (comparator.StartsWith("рав") || comparator == "=")
                comparatorPredicate = n => n == num;

            if (op == "и" || op == "но")
                filtration.SetAgeFilter(false, comparatorPredicate);
            else
                filtration.SetAgeFilter(true, comparatorPredicate);
        }

        static void AvgAgeFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!numberRegex.IsMatch(filter!.Value))
                return;
            var prop = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("prop")!.Value;
            var comparator = numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("comparator")!.Value;
            if (!float.TryParse(numberRegex.Match(filter!.Value).Groups.GetValueOrDefault("num")!.Value, out float num))
                return;
            if (prop != "продолжительность жизни")
                return;
            Predicate<float> comparatorPredicate = n => true;
            if (comparator == "больше" || comparator == ">")
                comparatorPredicate = n => n > num;
            else if (comparator == "меньше" || comparator == "<")
                comparatorPredicate = n => n < num;
            else if (comparator == "не больше" || comparator == "<=")
                comparatorPredicate = n => n <= num;
            else if (comparator == "не меньше" || comparator == ">=")
                comparatorPredicate = n => n >= num;
            else if (comparator == "не равно" || comparator == "!=")
                comparatorPredicate = n => n != num;
            else if (comparator.StartsWith("рав") || comparator == "=")
                comparatorPredicate = n => n == num;

            if (op == "и" || op == "но")
                filtration.SetAvgAgeFilter(false, comparatorPredicate);
            else
                filtration.SetAvgAgeFilter(true, comparatorPredicate);
        }

        static void ActiveFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!filter!.Value.Trim().ToLower().StartsWith("активн"))
                return;
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());
            if (op == "и" || op == "но")
                filtration.SetActiveFilter(false, notCond);
            else
                filtration.SetActiveFilter(true, notCond);
        }
        static void VaccineFilter(Filter filtration, string op, Group? not, Group? filter)
        {
            if (!filter!.Value.Trim().ToLower().StartsWith("вакцинирован"))
                return;
            var notCond = !string.IsNullOrEmpty(not!.Value.Trim().ToLower());
            if (op == "и" || op == "но")
                filtration.SetVaccineFilter(false, notCond);
            else
                filtration.SetVaccineFilter(true, notCond);
        }
        static void InitData()
        {
            using StreamReader reader = new("../../../data.txt");
            string? petLine;
            while ((petLine = reader.ReadLine()) != null)
            {
                var petProps = petLine.Split(';', StringSplitOptions.TrimEntries);
                var pet = new Pet
                {
                    Name = petProps[0],
                    Vacinied = int.Parse(petProps[1]) == 1,
                    Age = uint.Parse(petProps[2]),
                    AvgAge = uint.Parse(petProps[3]),
                    Active = int.Parse(petProps[4]) == 1,
                    Color = ulong.Parse(petProps[5]),
                    Conditions = ulong.Parse(petProps[6]),
                    Сompatibility = ~ulong.Parse(petProps[7]),
                    Size = float.Parse(petProps[8]),
                };
                var type = tree.Types.Where(t => t.Name == petProps[9]).FirstOrDefault();
                if (type == null)
                {
                    type = new() { Name = petProps[9] };
                    tree.Types.Add(type);
                }
                var specie = type.Species.Where(s => s.Name == petProps[10]).FirstOrDefault();
                if (specie == null)
                {
                    specie = new() { Name = petProps[10] };
                    type.Species.Add(specie);
                }
                var breed = specie.Breeds.Where(b => b.Name == petProps[11]).FirstOrDefault();
                if (breed == null)
                {
                    breed = new() { Name = petProps[11] };
                    specie.Breeds.Add(breed);
                }
                pet.Type = type;
                pet.Specie = specie;
                pet.Breed = breed;
                breed.Pets.Add(pet);
            }
        }
    }
}
