﻿// TODO identifier si l'étudiant est FE ou FA    pour notifier à l'utilisateur la necessité de signer
// TODO inserer un nouveau tableau (dans le json) pour chaque appel réalisé et incrementer de 1 à son nom (comme si ct le jour suivant) pour avoir un historique des appels
// TODO avoir des stat pour chaque eleves (toutes les absences, détail et périodes)
// TODO rendre l'application configurable via un fichier json
// TODO Recherche dichotomique pour modifier/supprimer une seule personne
// TODO Ajouter des tests unitaire

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Program
{
    // Création des listes d'eleves totaux et absents
    List<Person> persons = new List<Person>();
    List<Person> personsAbsentes = new List<Person>();

    //MDP de l'application 
    private const string passwordApp = "123";
    string? moreStudents = "";
    private bool quit = false;
    public void Run()
    {

        // TODO here lire avec lecteur json la liste d'étudiant dans personnes.json et mettre dans une variable pour utiliser dans les features listing/suppression
        while (!quit)
        {
            Console.WriteLine("\r\nMenu :\r\n");
            Console.WriteLine("1 - Faire l'appel");
            Console.WriteLine("2 - Lister tous les étudiants");
            Console.WriteLine("3 - Ajouter une liste d'étudiants");
            Console.WriteLine("4 - Supprimer la liste actuelle d'étudiants");
            Console.WriteLine("5 - Lister l'historique des appels");
            Console.WriteLine("6 - Quitter l'application");

            string? choice = Console.ReadLine();
            Console.Clear();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Liste de tous les étudiants dans la classe : ");
                    try{
                        faireAppel();
                        clearPrompt();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur : {ex.Message}");
                    }
                    break;

                case "2":
                    Console.WriteLine("Liste de tous les étudiants dans la classe : ");
                    listStudent();
                    clearPrompt();
                    break;

                case "3":
                    addListStudents();
                    break;

                case "4":
                    deleteStudents();
                    clearPrompt();
                    break;

                case "6":
                    quit = true;
                    break;

                default:
                    Console.WriteLine("Choix non valide, veuillez réessayer.");
                    break;
            }
        }
    }

    public void faireAppel()
    {
        // Boucler sur le tableau pour voir si la data dans 'présence' est true ou false
        foreach (Person etudiant in persons)
        {
            // Si c'est false, alors on ajoute l'étudiant dans un nouveau tableau "arrayAbsent"
            if (etudiant.Presence == false)
            {
                personsAbsentes.Add(etudiant);
            }
        }
        if (personsAbsentes.Count == 0){
            if(persons.Count!= 0){
                Console.WriteLine("Tous les élèves sont présents ! :)");
            }
            else{
                Console.WriteLine("Il n'y a pas d'éléve inscrit");
            }
        }
        else{
            Console.WriteLine("Elève(s) absent(e)(s) : ");
            // Boucler sur le tableau "arrayAbsent" pour afficher tous les élèves absents avec un print
            foreach (Person personAbsente in personsAbsentes){
                // Ecriture des élèves absents 
                Console.WriteLine(personAbsente.FirstName + " " + personAbsente.LastName);
            }
        }
    }

    public void listStudent(){

        if (persons.Count == 0){
            Console.WriteLine("Il n'y a pas d'éléve inscrit");
        }
        else{
            // Boucler sur le tableau "persons" pour afficher tous les élèves avec un print
            foreach (Person student in persons){
                // Ecriture des élèves absents 
                Console.WriteLine(student.FirstName + " " + student.LastName);
            }
        }
    }

    public void addListStudents(){

        // Regarder dans le fichier json si y'a deja une liste, si y'a une liste alors on en créer une autre à la suite
        do{
            // Rentrée des utilisateurs dans la feuille d'appel
            Console.Write("Entrer le nom: ");
            string? firstName = Console.ReadLine();
            Console.Write("Entrer le Prénom: ");
            string? lastName = Console.ReadLine();
            Console.Write("Présence (ne rien rentrer si pas présent): ");
            string? presence = Console.ReadLine();

            // Créer le nombre d'utilisateurs (objets) présents dans la promotion comportant leurs infos (nom prénom présence) et les insérer dans une liste
            if (String.IsNullOrEmpty(presence))
            {
                persons.Add(new Person() { FirstName = firstName, LastName = lastName, Presence = false });
            }
            else
            {
                persons.Add(new Person() { FirstName = firstName, LastName = lastName, Presence = true });
            }

            Console.Write("Rentrer plus d'étudiants ? (ne rien rentrer si on ne veut pas inscrire plus d'étudiants)");
            moreStudents = Console.ReadLine();
            Console.Clear();
        } while (!String.IsNullOrEmpty(moreStudents));

        //insertion dans json des etudiants
        string jsonFilePath = "personnes.json"; // Spécifiez le chemin du fichier JSON
        var json = JsonSerializer.Serialize(persons);

        File.WriteAllText(jsonFilePath, json);
    }

    public void deleteStudents(){
        persons.Clear();
        personsAbsentes.Clear();
        Console.WriteLine("Tout les etudiants ont été supprimés de la liste ! ");
    }

    public void clearPrompt(){
        Console.WriteLine("\r\nAppuyer sur entrer pour revenir en arriere");
        Console.ReadLine();
        Console.Clear();
    }

    public class Person{
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public bool Presence { get; set; }
    }

    public static void Main(string[] args){
        bool correctPaswword = false;
        while (!correctPaswword){
            Console.Write("Veuillez entrer le mot de passe :");
            string passwordAsk = Console.ReadLine();
            if (passwordApp == passwordAsk){
                correctPaswword = true;
            }
            else{
                Console.WriteLine("Mot de passe incorrect. Veuillez réessayer.");
            }
        }
        Program program = new Program();
        program.Run();
    }
}
