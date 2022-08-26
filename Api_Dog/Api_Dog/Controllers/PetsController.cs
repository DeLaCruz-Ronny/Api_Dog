using Api_Dog.Context;
using Api_Dog.DTOs;
using Api_Dog.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_Dog.Controllers
{
    [ApiController]
    [Route("api/pets")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PetsController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<List<DogDTO>> Get()
        {
            var pet = await context.dogs.ToListAsync();
            return mapper.Map<List<DogDTO>>(pet);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<DogDTO>> Get(int Id)
        {
            var pets = await context.dogs.FirstOrDefaultAsync(x => x.Id == Id);

            if (pets == null)
            {
                return BadRequest();
            }

            return mapper.Map<DogDTO>(pets);
        }

        [HttpGet("{Nombre}")]
        public async Task<ActionResult<List<DogDTO>>> Get(string Nombre)
        {
            var pets = await context.dogs.Where(x => x.Nombre.Contains(Nombre)).ToListAsync();

            if (pets == null)
            {
                return BadRequest();
            }

            return mapper.Map<List<DogDTO>>(pets);
        }

        [HttpPost]
        public async Task<ActionResult> Post(DogsDTO dogsDTO)
        {
            var pets = mapper.Map<Dog>(dogsDTO);
            context.Add(pets);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> put(Dog dog, int Id)
        {
            if (dog.Id != Id)
            {
                return BadRequest("No se encontro en Id");
            }

            var existe = await context.dogs.AnyAsync(x => x.Id == Id);

            if (!existe)
            {
                return BadRequest();
            }

            context.Update(dog);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult> Delete(int Id)
        {
            var existe = await context.dogs.AnyAsync(x => x.Id == Id);

            if (!existe)
            {
                return BadRequest();
            }

            context.Remove(new Dog { Id = Id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
