﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RosCottedge.Models
{
    public class AddDatabaseAtStart : DropCreateDatabaseAlways<SiteContext>
    {
        protected override void Seed(SiteContext db)
        {
            db.Users.Add(new User
            {
                FirstName = "Адам",
                MiddleName = "Дуглас",
                LastName = "Драйвер",
                Email = "adam@gmail.com",
                Phone = "+79001234567890",
                Login = "adam",
                Password = "adam"
            });

            db.SaveChanges();

            db.Houses.Add(new House
            {
                Name = "Дача в аренду",
                Price = 30000,
                NumberOfPersons = 18,
                Region = "Республика Карелия",
                Locality = "Петрозаводск",
                Area = "Екатерининская улица",
                HouseNumber = "9А",
                Description = "Для бронирования используйте красную кнопку Забронировать сейчас на этой странице сайта. Бронирование на лето открыто. Это - гостевой дом класса люкс в 15 минутах езды от Петрозаводска. Вместимость 10+ человек. Свой берег живописного озера. Красивые виды со всех сторон. Отсутствие близких соседей.",
                Food = "Имеется своя кухня.",
                Transfer = "Своим транспортом.",
                ServicesIncluded = "Пользование всем, что есть в доме.",
                AdditionalServices = "Пользование катером.",
                Accomodations = "Въезд - с 16:00, выезд не позже 13:00.",
                BookingConditions = "Оговариваются с владельцем.",
                Rating = 0,
                UserId = 1
            });

            db.Houses.Add(new House
            {
                Name = "Дача в аренду",
                Price = 30000,
                NumberOfPersons = 18,
                Region = "Республика Карелия",
                Locality = "Петрозаводск",
                Area = "Екатерининская улица",
                HouseNumber = "9А",
                Description = "Для бронирования используйте красную кнопку Забронировать сейчас на этой странице сайта. Бронирование на лето открыто. Это - гостевой дом класса люкс в 15 минутах езды от Петрозаводска. Вместимость 10+ человек. Свой берег живописного озера. Красивые виды со всех сторон. Отсутствие близких соседей.",
                Food = "Имеется своя кухня.",
                Transfer = "Своим транспортом.",
                ServicesIncluded = "Пользование всем, что есть в доме.",
                AdditionalServices = "Пользование катером.",
                Accomodations = "Въезд - с 16:00, выезд не позже 13:00.",
                BookingConditions = "Оговариваются с владельцем.",
                Rating = 0,
                UserId= 1
            });

            db.Houses.Add(new House
            {
                Name = "Дача в аренду",
                Price = 30000,
                NumberOfPersons = 18,
                Region = "Республика Карелия",
                Locality = "Петрозаводск",
                Area = "Екатерининская улица",
                HouseNumber = "9А",
                Description = "Для бронирования используйте красную кнопку Забронировать сейчас на этой странице сайта. Бронирование на лето открыто. Это - гостевой дом класса люкс в 15 минутах езды от Петрозаводска. Вместимость 10+ человек. Свой берег живописного озера. Красивые виды со всех сторон. Отсутствие близких соседей.",
                Food = "Имеется своя кухня.",
                Transfer = "Своим транспортом.",
                ServicesIncluded = "Пользование всем, что есть в доме.",
                AdditionalServices = "Пользование катером.",
                Accomodations = "Въезд - с 16:00, выезд не позже 13:00.",
                BookingConditions = "Оговариваются с владельцем.",
                Rating = 0,
                UserId = 1
            });

            db.Houses.Add(new House
            {
                Name = "Дача в аренду",
                Price = 30000,
                NumberOfPersons = 18,
                Region = "Республика Карелия",
                Locality = "Петрозаводск",
                Area = "Екатерининская улица",
                HouseNumber = "9А",
                Description = "Для бронирования используйте красную кнопку Забронировать сейчас на этой странице сайта. Бронирование на лето открыто. Это - гостевой дом класса люкс в 15 минутах езды от Петрозаводска. Вместимость 10+ человек. Свой берег живописного озера. Красивые виды со всех сторон. Отсутствие близких соседей.",
                Food = "Имеется своя кухня.",
                Transfer = "Своим транспортом.",
                ServicesIncluded = "Пользование всем, что есть в доме.",
                AdditionalServices = "Пользование катером.",
                Accomodations = "Въезд - с 16:00, выезд не позже 13:00.",
                BookingConditions = "Оговариваются с владельцем.",
                Rating = 0,
                UserId = 1
            });

            base.Seed(db);
        }
    }
}