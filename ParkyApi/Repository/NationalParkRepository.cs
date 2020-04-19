using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyApi.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        private readonly ApplicationDbContext _db;

        public NationalParkRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            this._db.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            this._db.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int Id)
        {
            return this._db.NationalParks.FirstOrDefault(a => a.Id == Id);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return this._db.NationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExists(string name)
        {
            bool value = this._db.NationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExists(int id)
        {
            return this._db.NationalParks.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return this._db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            this._db.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
