import {TipKaroserije} from "./TipKaroserije.js";
import { AutoPlac} from "./AutoPlac.js";

var listaTipovaK=[];
fetch("https://localhost:5001/TipKaroserije/PreuzmiTipKaroserije")
.then(p=>{
    p.json().then(tipoviKaroserija=>{
        tipoviKaroserija.forEach(tipK => {
            var tK=new TipKaroserije(tipK.id, tipK.naziv, tipK.opis);
            listaTipovaK.push(tK); 
        });
        fetch("https://localhost:5001/AutoPlac/PreuzmiAutoPlaceve")
        .then(p=>{
            p.json().then(placevi=>{
                placevi.forEach(el=>{
                    var autoPlac=new AutoPlac(listaTipovaK,el.id,el.naziv,el.telefon,el.adresa,el.kapacitet);
                    autoPlac.crtaj(document.body);
                })
            })
        })
    })
})
