// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const animals = [
    { name: "garfield", species: "cat", class: { name: "mamalia" } },
    { name: "nemo", species: "fish", class: { name: "invertebrata" } },
    { name: "tom", species: "cat", class: { name: "mamalia" } },
    { name: "garry", species: "cat", class: { name: "mamalia" } },
    { name: "dory", species: "fish", class: { name: "invertebrata" } },
    { name: "oren", species: "cat", class: { name: "mamalia" } },
    { name: "meong", species: "cat", class: { name: "mamalia" } },
    { name: "lele", species: "fish", class: { name: "invertebrata" } }

]
const onlyCat = [];
for (var i = 0; i < animals.length; i++) {
    if (animals[i].species=="cat") {
        onlyCat.push(animals[i])
    }
}
console.log(onlyCat);

for (var i = 0; i < animals.length; i++) {
    if (animals[i].species == "fish") {
        animals[i].class.name = "non mamalia";
    }
}
console.log(animals);