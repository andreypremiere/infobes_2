using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSALibrary
{
    public class RSAlib
    {
        private readonly BigInteger _min = BigInteger.Pow(10, 13);  // Задаем максимальное и минимальное значения для p и q
        private readonly BigInteger _max = BigInteger.Pow(10, 14) - 1;

        private const int _neededCountOfFigure = 28;  // Количество символов в n

        private BigInteger _n; // Состоит из 28 десятичных знаков (от 10^27 до 10^28 - 1).
                       // p и q должны быть такими, чтобы p * q = n

        public BigInteger _p;
        public BigInteger _q;
        private BigInteger _d; // = (p - 1) * (q - 1) функцией Эйлера для числа n
        private BigInteger _s; // закрытая экспонента e * s mod d = 1
        private BigInteger _e; // открытая экспонента 1 < e < d && e и d взаимно простые

        // Конструктор для класса
        public RSAlib() {
            GenerateNumber generateNumber = new GenerateNumber();

            // Генерирование чисел p и q такие, чтобы удовлетворяло условию n = p * q
            BigInteger p = generateNumber.GenerateRandomPrimeInRange(_min, _max);
            BigInteger q = generateNumber.GenerateRandomPrimeInRange(_min, _max);

            while (true)
            {
                if(checkValuesPandQ(p, q))
                {
                    break;
                }

                p = generateNumber.GenerateRandomPrimeInRange(_min, _max);
                q = generateNumber.GenerateRandomPrimeInRange(_min, _max);
            }

            this._n = p * q;
            this._p = p;
            this._q = q;
            Console.WriteLine($"Число p = {p}");
            Console.WriteLine($"Число q = {q}");
            Console.WriteLine($"Число n = {_n}");

            // Расчет функции Эйлера
            this._d = (p - 1) * (q - 1);
            Console.WriteLine($"Число d = {_d}");

            // Расчет закрытой экспоненты
            this._s = Calculate_s();
            Console.WriteLine($"Число s = {_s}");

            // Расчет открытой экспоненты
            this._e = Calculate_e(_s, _d);

            if (_e != -1)
            {
                Console.WriteLine($"Число e {_e}");
            }
            else
            {
                Console.WriteLine($"Число e не может быть вычеслено");
            }
        }

        public BigInteger GetN() { return _n; }

        public BigInteger Getd() { return _d; }

        public BigInteger Gets() { return _s; }

        public BigInteger Gete() { return _e; }

        /// <summary>
        /// Проверяет, имеет ли произведение чисел p и q нужное количество цифр.
        /// </summary>
        /// <param name="p">Первое число.</param>
        /// <param name="q">Второе число.</param>
        /// <returns>Возвращает true, если количество цифр произведения соответствует _neededCountOfFigure, иначе false.</returns>
        private bool checkValuesPandQ(BigInteger p, BigInteger q)
        {
            BigInteger n = p *  q;
            string numbers = n.ToString();
            int countFigure = numbers.Length;

            if (countFigure == _neededCountOfFigure)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Вычисляет число s, которое является взаимно простым числом с _d, удовлетворяющим условию 1 < s < _d.
        /// </summary>
        /// <returns>Возвращает найденное число s или 0, если оно не найдено.</returns>
        private BigInteger Calculate_s()
        {
            for (BigInteger i = _d - 1000; i > 1; i--)
            {
                if (AreCoprime(i, _d)) {
                    return i;
                }
            }

            Console.WriteLine("Не было найдено число s.");
            return 0;
        }

        /// <summary>
        /// Проверяет, являются ли два числа взаимно простыми (их наибольший общий делитель равен 1).
        /// </summary>
        /// <param name="a">Первое число.</param>
        /// <param name="b">Второе число.</param>
        /// <returns>Возвращает true, если числа взаимно простые, иначе false.</returns>
        private bool AreCoprime(BigInteger a, BigInteger b)
        {
            return GCD(a, b) == 1;
        }

        /// <summary>
        /// Вычисляет наибольший общий делитель (НОД) двух чисел с использованием алгоритма Евклида.
        /// </summary>
        /// <param name="a">Первое число.</param>
        /// <param name="b">Второе число.</param>
        /// <returns>Возвращает НОД чисел a и b.</returns>
        private BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Вычисляет модульное мультипликативное обратное числа a по модулю m с использованием расширенного алгоритма Евклида.
        /// </summary>
        /// <param name="a">Число для нахождения обратного.</param>
        /// <param name="m">Модуль.</param>
        /// <returns>Возвращает модульное мультипликативное обратное a или 0, если обратное не существует.</returns>
        private BigInteger Calculate_e(BigInteger s, BigInteger d)
        {
            // Расширенный алгоритм Евклида
            BigInteger m0 = d, y = 0, x = 1;

            if (d == 1)
                return 0; // Обратного числа нет, если d == 1

            // Алгоритм для нахождения обратного числа
            while (s > 1)
            {
                // q — это частное от деления s на d
                BigInteger q = s / d;
                BigInteger t = d;

                // d — это остаток от деления s на d
                d = s % d;
                s = t;
                t = y;

                // Обновляем значения для x и y
                y = x - q * y;
                x = t;
            }

            // Если x < 0, добавляем m0, чтобы результат был положительным
            if (x < 0)
                x += m0;

            return x;
        }

        /// <summary>
        /// Шифрует строку с использованием алгоритма RSA.
        /// </summary>
        /// <param name="message">Сообщение для шифрования.</param>
        /// <returns>Возвращает зашифрованное сообщение в виде строки с числами, разделенными пробелами.</returns>
        public string EncryptString(string message)
        {
            List<ulong> ulongs = ConvertToListNumbers(message);

            List<BigInteger> figuresRSA = new List<BigInteger>();

            string encryptMessage = "";

            for (int i = 0; i < ulongs.Count; i++)
            {
                BigInteger numRSA = BigInteger.ModPow(ulongs[i], _e, _n);
                figuresRSA.Add(numRSA);
            }

            for (int i = 0; i < figuresRSA.Count; i++)
            {
                encryptMessage += (figuresRSA[i] + " ");
            }

            return encryptMessage;
        }

        /// <summary>
        /// Дешифрует зашифрованное сообщение, используя алгоритм RSA.
        /// </summary>
        /// <param name="encryptedMessage">Зашифрованное сообщение в виде строки с числами, разделенными пробелами.</param>
        /// <returns>Возвращает расшифрованное сообщение.</returns>
        public string DecryptString(string encryptedMessage)
        {
            string[] encryptedValues = encryptedMessage.Split(' ');
            List<BigInteger> encryptedNumbers = new List<BigInteger>();

            foreach (string value in encryptedValues)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    encryptedNumbers.Add(BigInteger.Parse(value));
                }
            }

            string decryptedMessage = "";
            List<ulong> ulongsNumbers = new List<ulong>();

            foreach (BigInteger encryptedNumber in encryptedNumbers)
            {
                ulong decryptedNumber = (ulong) BigInteger.ModPow(encryptedNumber, _s, _n);
                ulongsNumbers.Add(decryptedNumber);
            }

            decryptedMessage = ConvertToMessage(ulongsNumbers);

            return decryptedMessage.ToString();
        }

        /// <summary>
        /// Преобразует строковое сообщение в список чисел типа ulong, разбивая сообщение на сегменты по 8 байт.
        /// </summary>
        /// <param name="message">Сообщение для преобразования.</param>
        /// <returns>Возвращает список чисел типа ulong, представляющих сообщение.</returns>
        private List<ulong> ConvertToListNumbers(string message)
        {
            byte[] byteArray = Encoding.Unicode.GetBytes(message);

            List<ulong> resultList = new List<ulong>();
            byte[] newNum = new byte[8];
            int byteIndex = 0;

            for (int i = 0; i < byteArray.Length; i++)
            {
                newNum[byteIndex] = byteArray[i];
                byteIndex++;

                if (byteIndex == 8)
                {
                    ulong unsignedNumber = BitConverter.ToUInt64(newNum, 0);
                    resultList.Add(unsignedNumber);

                    newNum = new byte[8];
                    byteIndex = 0;
                }
            }

            if (byteIndex > 0)
            {
                for (int j = byteIndex; j < 8; j++)
                {
                    newNum[j] = 0; 
                }
                ulong unsignedNumber = BitConverter.ToUInt64(newNum, 0);
                resultList.Add(unsignedNumber);
            }

            return resultList;
        }

        /// <summary>
        /// Преобразует список чисел типа ulong обратно в строковое сообщение.
        /// </summary>
        /// <param name="ulongList">Список чисел типа ulong для преобразования.</param>
        /// <returns>Возвращает сообщение в виде строки.</returns>
        private string ConvertToMessage(List<ulong> ulongList)
        {
            List<byte> byteList = new List<byte>();

            foreach (ulong value in ulongList)
            {
                byte[] byteArray = BitConverter.GetBytes(value);
                byteList.AddRange(byteArray);
            }

            byte[] byteArrayFull = byteList.ToArray();

            string message = Encoding.Unicode.GetString(byteArrayFull).TrimEnd('\0');

            return message;
        }


    }
}
