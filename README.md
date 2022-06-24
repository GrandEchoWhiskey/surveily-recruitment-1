###### Language: Polish

Projekt aplikacji konsolowej w .NET 5.0, która w najoptymalniejszy sposób dla danej maszyny pobierze dane JSON z adresów URL zadanych na stdin.
Zaprojektowane klasy i testy jednostkowe.

#### Funkcjonalności aplikacji:
- Przyjęcie listy adresów URL na wejściu oddzielonych średnikiem,
- Przyjęcie folderu docelowego, na zapisanie wyników pracy,
- Weryfikacja czy zadane adresy są prawdziwymi URL,
- Weryfikacja czy zadane adresy istnieją w sieci,
- Pobranie danych,
- Zapis pobranych danych w folderze docelowym.

#### Wydajność:
Do projektu użyłem pracę w wątkach, co może znacznie przyspieszyć pobieranie. Nie są zapisywane po kolei(szeregowo), lecz równolegle. Dzięki takiemu rozwiązaniu inne pliki nie muszą czekać w kolejce do pobrania, gdy jeden z adresów nie odpowiada.
```csharp
ThreadStart thread_start = new (() => checkAndDownload(url, path, name));
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

#### Optymalność:
Poniższy kod tworzy kopię danych ze strony internetowej i zapisuje w podanym pliku. Użyłem metody (StreamReader)ReadToEnd(), która na chwilę przechowuje wszystkie dane z danej strony w pamięci. Może to doprowadzić do spowolnienia w przypadku dużej ilości danych, lub małej ilości pamięci. Rozwiązaniem tego problemu mogło by być przydzielenie bufforu i wykonywania operacji w pętli.
W praktyce rzadko spotyka się duże pliki json, ponieważ są one mało wydajne w porównaniu do baz danych (SQL).
```csharp
byte[] data_byte_array = new UTF8Encoding(true).GetBytes(stream_reader.ReadToEnd());
this._fstream.Write(data_byte_array, 0, data_byte_array.Length);
```
