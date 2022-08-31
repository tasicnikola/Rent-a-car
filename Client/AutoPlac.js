import { VoziloInfo } from "./VoziloInfo.js";

export class AutoPlac{
    constructor(listaTipovaKaroserije,id, ime, brojTelefona, adresa, kapacitet)
    {
        this.id=id;
        this.naziv=ime;
        this.brojTelefona=brojTelefona;
        this.adresa=adresa;
        this.kapacitet=kapacitet;
        this.listaTipovaKaroserije=listaTipovaKaroserije;
        this.konteiner=null;
    }
    crtaj(host){
        this.konteiner=document.createElement("div"); 
        this.konteiner.className="GlavnijiKonteiner";
        host.appendChild(this.konteiner);

                
        let placInfo=document.createElement("div");
        placInfo.className="placInfo";
        this.konteiner.appendChild(placInfo);

        let placTelefon=document.createElement("div");
        placTelefon.className="placTelefon";
        placTelefon.innerHTML="Telefon: "+this.brojTelefona;
        placInfo.appendChild(placTelefon);

        let placIme=document.createElement("div");
        placIme.className="placIme";
        placIme.innerHTML=this.naziv;
        placInfo.appendChild(placIme);

        let placAdresa=document.createElement("div");
        placAdresa.className="placAdresa";
        placAdresa.innerHTML="Adresa: "+this.adresa;
        placInfo.appendChild(placAdresa);

        var glavniKonteiner=document.createElement("div");  
        glavniKonteiner.className="GlavniKonteiner";
        this.konteiner.appendChild(glavniKonteiner);

        let kontForma=document.createElement("div");     
        kontForma.className="Forma";
        glavniKonteiner.appendChild(kontForma);

        this.crtajFormu(kontForma);         
    }
    crtajRed(host)
    {
        let red=document.createElement("div");
        red.className="red";
        host.appendChild(red);
        return red;

    }
    crtajFormu(host){
        let red=this.crtajRed(host);
        let l=document.createElement("label");
        l.innerHTML="Tip karoserije: ";
        red.appendChild(l);                    

        let se=document.createElement("select");  
        red.appendChild(se);

        let op;
        this.listaTipovaKaroserije.forEach(tipKaroserije=>{ 
            op=document.createElement("option");           
            op.innerHTML=tipKaroserije.naziv;
            op.value=tipKaroserije.id;
            se.appendChild(op);              
        })

        let btnNadji=document.createElement("button");
        btnNadji.innerHTML="Pretrazi";
        btnNadji.onclick=(ev)=>this.pretraziPoOblikuKaroserije();
        red.appendChild(btnNadji);

        let prikazVozila=document.createElement("div");
        prikazVozila.className="PrikazVozila";
        var parent=this.konteiner.querySelector(".GlavniKonteiner");;
        parent.appendChild(prikazVozila);

        red=this.crtajRed(host);
        l=document.createElement("label");
        l.innerHTML="Registarska tablica: ";
        red.appendChild(l);
        var reg_tablica=document.createElement("input");
        reg_tablica.className="tablica";
        red.appendChild(reg_tablica);

        red=this.crtajRed(host);
        l=document.createElement("label");
        l.innerHTML="Cena: ";
        red.appendChild(l);
        var cena=document.createElement("input");
        cena.className="Cena";
        red.appendChild(cena);

        var info=["Marka: ", "Model: ","Godina proizvodnje: ","Kilometraza: ","Zapremina motora: ", "Snaga motora: ", "brLK: "];
        info.forEach(atr => {
            red=this.crtajRed(host);
            l=document.createElement("label");
            l.innerHTML=atr;
            red.appendChild(l);
            var inp=document.createElement("input");
            inp.className="vidljivo";
            red.appendChild(inp);
            
        });

        red=this.crtajRed(host);
        red.className="Dugmici";
        let btnDodaj=document.createElement("button");
        btnDodaj.innerHTML="Dodaj";
        btnDodaj.className="vidljivo";
        red.appendChild(btnDodaj);
        btnDodaj.onclick=(ev)=>this.zaDodavanje(reg_tablica.value,cena.value,this.vratiListu())
        var opis=document.createElement("div");
        opis.innerHTML="";
        opis.className="Opis";
        this.konteiner.appendChild(opis);
    }
    zaDodavanje(tab,cena,lista){
        this.dodajVozilo(lista[0].value,lista[1].value,tab,cena,lista[2].value,lista[3].value,lista[4].value,lista[5].value,this.naziv, lista[6].value);
    }
    vratiListu(){
        return this.konteiner.querySelectorAll("input.vidljivo");
    }
    pretraziPoOblikuKaroserije(){
        let optionEl = this.konteiner.querySelector("select");
        var TipKaID=optionEl.options[optionEl.selectedIndex].value;
        var info;
        this.listaTipovaKaroserije.forEach(el=>{
            if(el.id==TipKaID){
                info=el.naziv+" - "+el.opis;
            }
        })
        fetch("https://localhost:5001/Vozilo/PrikazPoObKaroserije/"+TipKaID+"/"+this.naziv,
        {
            method:"GET"
        }).then(s=>{
            if(s.ok){
                var zaVozila=this.obrisiPrethodniSadrzaj();
                s.json().then(data=>{
                    data.forEach(v=>{
                        let vozilo=new VoziloInfo(v.id, v.marka, v.model, v.godinaProizvodnje, v.imeVlasnika, v.brojTelefona, v.tablica, v.cena);
                        vozilo.crtaj(this.konteiner.querySelector(".PrikazVozila"));
                    })
                })
            }
            var opis=this.konteiner.querySelector(".Opis");
            opis.innerHTML=info;
        })

    }
    obrisiPrethodniSadrzaj()
    {
        var deoZaVozila=this.konteiner.querySelector(".PrikazVozila");
        var roditelj=deoZaVozila.parentNode;
        roditelj.removeChild(deoZaVozila);

        let prikazVozila=document.createElement("div");
        prikazVozila.className="PrikazVozila";
        roditelj.appendChild(prikazVozila);
        return prikazVozila;
    }
    dodajVozilo(marka, model, tablica, cena, godina_proiz, kilometraza, zap_motora, snaga_motora, plac_naziv, vlasnik_brLK)
    {
        var letters = /^[a-zA-Z\s]*$/;
        var numbers = /^[0-9]*$/;
        if(marka==""){
            alert("Marka automobila je obavezno polje!");
            return;
        }
        else{//
            if(marka.length>15 || !marka.match(letters)){
                alert("Marka ne sme da bude duza od 15 karaktera niti da sadrzi cifre ili specijalne znake!");
                return;
            }
        }
        if(model==""){
            alert("Unesite model automobila");
            return
        } 
        else{
            if(model.length>15){
                alert("Naziv modela ne sme da bude duzi od 15 karaktera")
            }
        }
        if(tablica==""){
            alert("Unesite broj registarskih tablica!");
            return;
        }
        else{
            if(tablica.length>8)
            {
                alert("Registarska oznaka vozila ne sme da bude duza od 8 karaktera!");
                return;
            }
        }
        if(cena=="")
        {
            alert("Unesite cenu!");
            return;
        }
        else{
            if(cena>200000 || cena<100 || !cena.match(numbers)){
                alert("Cena mora da bude u opsegu 100-200000! i sme da sadrzi samo brojeve");
                return;
            }
        }
        if(godina_proiz=="")
        {
            alert("Unesite godinu proizvodnje!");
            return;
        }
        else{
            if(godina_proiz<1960 || godina_proiz>2021 || !godina_proiz.match(numbers)){
            alert("Vozilo ne moze da bude proizvedeno pre 1960-te godine! Godina je cetvorocifreni broj bez specijalnih znakova");
            return;
            }
        }
        if(!kilometraza.match(numbers) || kilometraza=="" )
        {
            alert("Unesite broj predjenih kilometara!");
            return;
        }
        if(!zap_motora.match(numbers) || zap_motora=="")
        {
            alert("Zapremina motora sme da sadrzi samo cifre!");
            return;
        }
        else{
            if(zap_motora<20 || zap_motora>8000)
            {
                alert("Zapremina motora mora da bude u opsegu 20-8000ccm!");
                return;
            }
        }
        if(!snaga_motora.match(numbers) || snaga_motora=="")
        {
            alert("Snaga sme da sadrzi samo cifre!");
            return;
        }
        else{
            if(snaga_motora<20 || snaga_motora>1000)
            {
                alert("Snaga motora mora da bude u opsegu 20-1000ks!");
                return;
            }
        }
        if(!vlasnik_brLK.match(numbers) || vlasnik_brLK=="")
        {
            alert("BRLK vlasnika sme da sadrzi samo cifre!");
            return;
        }
        else{
            if(vlasnik_brLK.length>9){
            alert("Broj LK je predug!");
            return;
            }
        }
        let optionEl = this.konteiner.querySelector("select");
        var TipKaStr=optionEl.options[optionEl.selectedIndex].innerHTML;
        fetch("https://localhost:5001/Vozilo/DodajVozilo/"+marka+"/"+model+"/"+tablica+"/"+cena+"/"+godina_proiz+"/"+kilometraza+"/"
        +zap_motora+"/"+snaga_motora+"/"+TipKaStr+"/"+this.naziv+"/"+vlasnik_brLK,
        {
            method:"POST"
        }).then(s=>{
            if(s.status==200){
                var zaVozila=this.obrisiPrethodniSadrzaj();
                s.json().then(data=>{
                    data.forEach(voz=>{
                        const novoVozilo= new VoziloInfo(voz.id, voz.marka,voz.model,voz.godinaProizvodnje,voz.imeVlasnika,voz.brojTelefona,voz.tablice, voz.cena);
                        novoVozilo.crtaj(this.konteiner.querySelector(".PrikazVozila"));
                    })

                })
            }
            else{
                if(s.status==203){
                    alert("U bazi vec postoji vozilo sa datom registarskom oznakom!");
                }
                if(s.status==204){
                 alert("Korisnik sa datim brojem licne karte ne postoji!");
                 }
            }
        })


    }
}