# Scanner

Самсонов Никита

Задание выполнение с использованием .NET Core в среде Rider.

Сервер выполняет поиск подозрительных строк по файлам, используя алгоритм Ахо-Корасик. 
Задачи по сканированию директорий выполняются параллельно.

<table>
<thead><tr><th>Method</th><th>Mean</th><th>Error</th><th>StdDev</th>
</tr>
</thead><tbody><tr><td>Naive</td><td>614.2 ms</td><td>14.00 ms</td><td>40.62 ms</td>
</tr><tr><td>Aho</td><td>1,275.5 ms</td><td>35.83 ms</td><td>101.64 ms</td>
</tr></tbody></table>
