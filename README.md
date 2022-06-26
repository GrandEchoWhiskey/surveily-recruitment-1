###### Language: Polish &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Framework: .NET 5.0 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Programming Language: C#

[![](https://img.icons8.com/color/25/000000/c-sharp-logo.png)](#)

Projekt aplikacji konsolowej w .NET 5.0, która w najoptymalniejszy sposób dla danej maszyny pobierze dane JSON z adresów URL zadanych na stdin.
Zaprojektowane klasy i testy jednostkowe.

#### Funkcjonalności aplikacji:
- Przyjęcie listy adresów URL na wejściu oddzielonych średnikiem,
- Przyjęcie folderu docelowego, na zapisanie wyników pracy,
- Weryfikacja czy zadane adresy są prawdziwymi URL,
- Weryfikacja czy zadane adresy istnieją w sieci,
- Pobranie danych,
- Zapis pobranych danych w folderze docelowym.

#### Wejście:
Do programu użytkownik wprowadza 3 wartości:
- Ciąg znaków reprezentujący adresy url źródeł oddzielonych średnikiem (możliwość uruchomienia z argumentem)
- Ciąg znaków reprezentujący ścieżkę zapisu
- Znak odpowiadający, czy dany plik ma przybrać orginalną nazwę
```csharp
string urls;
if (args.Length == 2)
{
  urls = args[1];
}
else
{
  Console.Write("Wpisz adresy URL oddzielone średnikiem: ");
  urls = Console.ReadLine();
}

Console.Write("Wpisz ścieżkę zapisu: ");
string dir = Console.ReadLine();

Console.Write("Używaj orginalnej nazwy pliku (t/n): ");
bool orginal_names = Console.ReadLine() != "n";
```

#### Wyjście:
Program tworzy pliki z danymi json pobranymi z adresów URL. Warunkiem są uprawnienia do folderu oraz prawidłowy i dostępny adres url z plikiem json.
```csharp
byte[] data_byte_array = new UTF8Encoding(true).GetBytes((StreamReader)stream_reader.ReadToEnd());
(FileStram)this._fstream.Write(data_byte_array, 0, data_byte_array.Length);
```

#### Struktura klas:
Klasy są przejrzyste, już po nazwach przychodzi na myśl - czym się zajmują.
```csharp
namespace Api
{
  public class Controller {}
  public class MyUrl {}
  public class MyDownloader {}
}
```

#### Optymalność pracy:
Testy URL pozwalają na przyspieszenie wyeliminowania adresów które nie mogą posiadać pliku json. Jest to sprawdzane poprzez sprawdzenie poprawności interfejsu webowego, oraz sprawdzeniu czy nie jest to numeryczny adres url. Następnie sprawdzane jest łącze do tego adresu.
```csharp
public class MyUrl
{
  public bool Is_UrlInterface();
  public bool Is_UrlIP();
  public bool Is_UrlPing();
}
```

#### Bezawaryjność:
Klasa MyDownloader otwiera `Stream` do `Response` strony sieciowej, oraz do pliku na urządzeniu hostowym klienta. Dodatkowo przy pobieraniu danych korzysta z dodatkowego `StreamReader` i `Stream` by skopiować dane z sieci do pliku. Jednak nie ma możliwości aby pozostały otwarte łącza przez które, inny wątek nie może kontynuować pracy.
```csharp
~MyDownloader()
{
  this.Close();
}

public void Close()
{
  this.Close_File();
  this.Close_Connection();
}
```

#### Wydajność:
Do projektu użyłem pracę w wątkach, co może znacznie przyspieszyć pobieranie. Nie są zapisywane po kolei(szeregowo), lecz równolegle. Dzięki takiemu rozwiązaniu inne pliki nie muszą czekać w kolejce do pobrania, gdy jeden z adresów nie odpowiada.
```csharp
ThreadStart thread_start = new (() => CheckAndDownload(url, path, name));
Thread thread = new (thread_start)
{
  Name = name,
  Priority = ThreadPriority.AboveNormal
};
thread.Start();
threadlist.Add(thread);
```

#### Przyjazna dla użytkownika:
Program ma możliwość wprowadzenia adresów poprzez przekazanie argumentów jak i przez konsolę.
```csharp
string urls;
if (args.Length == 2)
{
  urls = args[1];
}
else
{
  Console.Write("Wpisz adresy URL oddzielone średnikiem: ");
  urls = Console.ReadLine();
}
```
Istnieje również możliwość prostej zmiany nazwy pliku; domyślnie nazwa pliku jest taka sama jak nazwa pliku z URL, jednak wystarczy inaczej wywołać metodę aby pliki zapisywane były w postaci "download_{index}.json".
```csharp
string name = GetName(url);
if (name == null || !use_real_file_name)
  name = "download_" + (i + 1).ToString() + ".json";
```
