﻿using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;

namespace PokeAPI.NET
{
    /// <summary>
    /// Represents an instance of a Pokédex
    /// </summary>
    public class Pokedex : PokeApiType
    {
        /// <summary>
        /// Wether it should cache the pokédex or not
        /// </summary>
        public static bool ShouldCacheData = true;
        /// <summary>
        /// Gets the cached pokédex
        /// </summary>
        public static Pokedex CachedPokedex = null;

        Dictionary<int, NameUriPair> pokemon = new Dictionary<int, NameUriPair>();

        /// <summary>
        /// A big list of Pokemon as NameUriPairs within this Pokedex instance
        /// </summary>
        public Dictionary<int, NameUriPair> PokemonList
        {
            get
            {
                return pokemon;
            }
        }

        /// <summary>
        /// Gets an entry of the PokemonList as a Pokemon
        /// </summary>
        /// <param name="index">The index of the entry</param>
        /// <returns>The entry of the PokemonList as a Pokemon</returns>
        public Pokemon RefPokemon(int index)
        {
            return Pokemon.GetInstance(PokemonList[ID].Name);
        }

        /// <summary>
        /// Creates a new instance from a JSON object
        /// </summary>
        /// <param name="source">The JSON object where to create the new instance from</param>
        protected override void Create(JsonData source)
        {
            foreach (JsonData data in source["pokemon"])
            {
                string[] num = data["resource_uri"].ToString().Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                pokemon.Add(Convert.ToInt32(num[num.Length - 1]), ParseNameUriPair(data));
            }
        }

        /// <summary>
        /// Creates an instance of a Pokedex
        /// </summary>
        /// <returns>The created Pokedex instance</returns>
        public static Pokedex GetInstance()
        {
            if (CachedPokedex != null)
                return CachedPokedex;

            Pokedex p = new Pokedex();
            Create(DataFetcher.GetPokedex(), p);

            if (ShouldCacheData)
                CachedPokedex = p;

            return p;
        }
    }
}
