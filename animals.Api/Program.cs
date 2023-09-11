using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using animals.Api.entity;

var animals = new List<Animal>()
{
    new Animal()
    {
        Id = 1,
        Name = "wolf",
        Predator = true,
        Age = 4,
    },
    new Animal()
    {
        Id = 2,
        Name = "cow",
        Predator = false,
        Age = 2,
    },
    new Animal()
    {
        Id = 3,
        Name = "snake",
        Predator = true,
        Age = 1,
    }
};

const string str = "Animals";

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//var group = app.MapGet("/");

// Read
app.MapGet("/", () => animals);

app.MapGet("/animals/{id}", (int id) =>
{
    Animal? animal = animals.Find(animal => animal.Id == id);

    if (animal == null)
        return Results.NotFound("Sorry, we can't find this animal");

    return Results.Ok(animal);
}).WithName(str);

// Create
app.MapPost("/animals", (Animal animal) =>
{
    animal.Id = animals.Max(a => a.Id) + 1;
    animals.Add(animal);
    return Results.CreatedAtRoute(str, new { id = animal.Id }, animal);
});

// Update
app.MapPut("/animals/{id}", (int id, Animal updatedAnimal) =>
{
    Animal? existingAnimal = animals.Find(animal => animal.Id == id);
    if (existingAnimal == null)
    {
        return Results.NotFound("There is no animal with this ID");
    }
    existingAnimal.Name = updatedAnimal.Name;
    existingAnimal.Age = updatedAnimal.Age;
    existingAnimal.Predator = updatedAnimal.Predator;
    return Results.NoContent();
});

// Delete
app.MapDelete("/animals/{id}", (int id) =>
{
    Animal? existingAnimal = animals.Find(animal => animal.Id == id);
    if (existingAnimal != null)
    {
        animals.Remove(existingAnimal);
    }
    return Results.NoContent();
});

app.Run();
