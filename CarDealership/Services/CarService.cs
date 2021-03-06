using AutoMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using CarDealership.Models;
using CarDealership.Repositories;
using CarDealership.Helpers;

namespace CarDealership.Services
{
    public interface ICarService
    {
        List<Car> GetAll();
        Car GetById(Guid carId);
        void Create(Car carToCreate);
        Car Update(Car updatedCar);
        void Delete(Guid carId);
    }


    public class CarService : ICarService
    {
        #region Constants

        private readonly DealershipContext _context;
        private readonly AppSettings _appSettings;

        private readonly IMapper _mapper;

        #endregion

        public CarService(DealershipContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        #region Public Methods

        public List<Car> GetAll()
        {
            var cars = _context.Cars.OrderBy(c => c.Make).ToList();

            return cars;
        }

        public Car GetById(Guid carId)
        {
            var car = _context.Cars.SingleOrDefault(c => c.Id == carId);

            // Error checking
            if (car == null)
            {
                throw new AppException("Car not found");
            }

            return car;
        }

        public void Create(Car carToCreate)
        {
            _context.Cars.Add(carToCreate);
            _context.SaveChanges();
        }

        public Car Update(Car updatedCar)
        {
            if (!_context.Cars.Any(c => c.Id == updatedCar.Id))
            {
                throw new AppException("Car not found");
            }

            _context.Cars.Update(updatedCar);
            _context.SaveChanges();

            return updatedCar;
        }

        public void Delete(Guid carId)
        {
            var car = GetById(carId);

            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
