using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace ParkyApi.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            this._db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            this._db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int Id)
        {
            return this._db.Trails.Include(c => c.NationalPark).FirstOrDefault(a => a.Id == Id);
        }

        public ICollection<Trail> GetTrails()
        {
            return this._db.Trails.Include(c => c.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            bool value = this._db.Trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool TrailExists(int id)
        {
            return this._db.Trails.Any(a => a.Id == id);
        }

        public bool Save()
        {
            return this._db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            this._db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == npId).ToList();
        }
    }
}
