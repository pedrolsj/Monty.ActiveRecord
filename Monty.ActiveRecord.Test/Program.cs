using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Monty.ActiveRecord.Test.Entities;
using System.Globalization;
using Monty.ActiveRecord.Test.Views;

namespace Monty.ActiveRecord.Test
{
    class Program
    {
        /// <summary>
        /// Gets or sets the random names.
        /// </summary>
        /// <value>
        /// The random names.
        /// </value>
        public static string[] RandomNames { get; set; }

        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            Console.Title = "Monty.ActiveRecord Tester";

            RandomNames = "maria;jose;paulo;luiz;carlos;joão;eduardo;marcelo;antonio;luis;rodrigo;jorge;sérgio;alexandre;ricardo;andre;rafael;daniel;vera;leandro;fernando;márcia;marcos;roberto;fabio;patricia;claudio;márcio;sandra;marco;fernanda;adriana;claudia;diego;guilherme;renato;rogerio;anderson;leonardo;luciano;juliana;simone;vanessa;felipe;tiago;pedro;francisco;flavio;cristiano;nelson;aline;carla;cesar;gilberto;andrea;sonia;adriano;julio;jaqueline;fabiano;thiago;tânia;vinicius;cristiane;luciana;eliane;mario;maurício;jeferson;juliano;angela;mauro;daniela;denise;edson;renata;bruno;condomínio;viviane;suc.;lucas;luciane;isabel;gustavo;michele;leticia;sandro;adão;alex;henrique;rita;sucessão;helio;sonepar;marlene;raquel;gabriel;rosane;rosangela;priscila;silvia;regina;douglas;emerson;debora;mara;camila;fabiana;iara;geraldo;cristina;carmen;caroline;fabricio;jair;manoel;jaime;alessandra;lucia;paula;silvio;alberto;vitor;everton;cintia;valdir;clovis;regis;gabriela;solange;ronaldo;wagner;lisiane;andreia;tatiana;rosana;alvaro;mariana;bruna;terezinha;carolina;jairo;ivone;marisa;katia;silvana;nilton;rejane;rosa;helena;milton;nara;alessandro;beatriz;deise;neusa;janaina;wilson;daiane;enio;marta;laura;miguel;joel;josiane;natalia;elaine;suzana;matheus;alexsandro;ivan;vagner;renan;gerson;leda;airton;cleber;norma;jussara;celso;jane;fatima;vanderlei;raul;marina;tatiane;miriam;osmar;fabiane;edison;evandro;nadir;christian;roger;construtora;vilson;giovani;gisele;elisabete;flavia;charles;sabrina;marli;magda;barbara;vilmar;carmem;elizabeth;marilene;jessica;karina;gilmar;elisangela;amanda;mônica;augusto;lauro;valmir;teresinha;vilma;gilson;tais;michael;lucio;clarice;anna;marilia;roberta;sirlei;jean;mateus;moises;susana;janete;lourdes;joana;cecilia;marlon;catia;celia;willian;vania;marcus;juarez;sidnei;karine;adriane;danilo;hugo;darci;janice;americo;heloisa;luana;claudete;moacir;telmo;jurema;osvaldo;cleusa;dirceu;thais;ulbra;kelly;humberto;lilian;jonas;ieda;pablo;mariza;adilson;ismael;leila;mirian;ederson;everson;elton;eder;angelo;elias;nilza;josue;filipe;oscar;cassio;william;samuel;michelle;vladimir;danielle;alice;cristian;neuza;nestor;cond.;ezequiel;robson;ligia;valter;olga;cassia;denis;eunice;karen;therezinha;luiza;daniele;alan;maristela;roseli;arnaldo;arthur;decio;lidia;erico;daiana;andressa;margarete;victor;elisabeth;alzira;nair;iracema;otavio;gislaine;edgar;maicon;jonathan;clarissa;hilário;odete;marino;valeria;liziane;irene;rafaela;elisandra;franciele;arlete;roque;virginia;melissa;suelen;liliane;luci;ernani;michel;liane;jefferson;romulo;aida;milene;nilo;larissa;rubens;nubia;antonia;israel;nilson;berenice;nadia;sheila;lisandra;gladis;igor;angelica;dalva;uniao;waldir;litribevi;lilia;ademar;noemia;newton;zilda;alceu;carina;jones;diogo;veronica;david;ademir;martha;graziela;estela;joice;ione;jader;alcides;rubem;ruth;jacqueline;geni;francine;vasco;mari;julia;eneida;cicero;fabiola;espolio;reginaldo;cassiano;dilson;catiane;elza;orlando;mariangela;vanda;dione;norberto;cezar;romeu;clair;self;ivete;rozane;valquiria;carine;lenir;darcy;egon;noeli;ildo;gelson;saul;alfredo;valmor;valesca;neiva;alexsander;maira;magali;jurandir;teresa;francisca;alexandra;giovana;paola;jeronimo;adalberto;aldo;lais;ceres;vicente;eurico;neide;doris;goldsztein;ilza;valdecir;olinda;ronei;neli;sirley;rosemari;rose;joelma;aurea;adroaldo;jonatas;zaida;emilio;ester;bianca;lorena;salvador;sueli;celina;nilva;reni;walter;afonso;amelia;juliane;margarida;iolanda;dario;alisson;veja;amarildo;jucara;patrick;irma;artur;loiva;valdemar;jesus;giovanni;catarina;davi;vivian;gilmara;sara;cleonice;lourenço;isaac;tamires;livia;euclides;mariane;deisi;edilson;waldemar;elisiane;aurelio;glaci;tereza;nelci;ayrton;roselaine;olmiro;sebastiao;glenio;reinaldo;richard;giuliano;gloria;dilma;clara;guaraci;edmilson;derli;ernesto;lenita;elisete;clayton;tarcisio;evelise;marion;leonor;margareth;claudir;harry;sinara;diogenes;caixa;amaro;domingos;eliana;lazaro;leni;inacio;marlei;cleci;cleiton;gilda;claudiomiro;greice;alba;helen;caio;nilda;nelsi;arno;cinara;lucimar;alexandro;pierre;sindicato;ellen;casa;lidiane;odair;ariane;elvira;emanuel;tanira;rene;altair;jackson;rute;edemar;moacyr;valério;luisa;romilda;gilnei;anita;tomaz;cleomar;hamilton;diva;jaderson;joseane;yara;marcela;valdenir;daltro;eloir;ulisses;christine;benjamin;odilon;juares;elvio;geovane;armando;dalila;ariel;zilma;adair;vanderson;rosalia;maximiliano;elenara;jacson;dante;tulio;marciano;lelia;manuel;junior;liana;angelita;erondina;jandira;delci;alexsandra;erika;rosimeri;karla;pamela;mayara;ciro;elsa;edith;leonel;elio;rosemar;cristine;roselane;naira;elizete;juan;anelise;marilei;eloisa;basa;werner;haroldo;imobiliare;adelina;gisela;eugenio;jenifer;arlindo;ereni;jamerson;selma;betina;silmar;izabel;norton;marlise;rosi;tamara;equipe;planidata;giselda;getulio;maico;volnei;asav-;almir;rudimar;fagner;sidney;glacy;ramon;herminio;rosani;adelia;cleuza;rossana;cibele;frederico;caren;ubirajara;nathalia;joaquim;andrei;santa;jonatan;plinio;janine;elisa;marines;claiton;rochele;gildo;octavio;sidinei;felix;ubiratan;celson;mary;evelin;benin;hilton;dulce;ines;noely;salete;cinthia;protásio;diane;murilo;heitor;darlan;rosemary;eliseu;nelcy;scheila;taise;vitória;tadeu;nicolás;marly;denilson;astor;jesse;suzane;ilma;egidio;dinora;dorli;odir;lourival;liliana;kleber;jason;benno;dinara;theo;olivia;aristeu;eroni;christiane;nedio;augusta;odacir;iran;penelope;gentil;marcolino;ivani;soraya;rudinei;gislene;ughini;conceicao;eloa;ingrid;iris;grimon;alda;eliandro;andresa;elis;etelvina;saulo;gessy;inez;georgia;clever;otilia;banrisul;ivana;lisete;adelino;ires;enilda;sylvio;bernardete;marjane;serno;gilvan;jocemar;taiana;jacob;natalino;graziele;grasiele;itamar;telma;lindomar;gian;meri;dinarte;laercio;doralice;dari;cristiana;gertrudes;erica;juracy;ariovaldo;iria;hilda;morgana;valdones;waldomiro;neri;wanda;marcelle;giovane;claudino;lurdes;irineu;lottici;janio;joyce;jayme;celio;rivelino;caetano;arduino;dayane;micheli;cely;liria;bernardo;priscilla;henry;raimundo;anivaldo;odemir;elma;geovani;josias;severino;otto;marc;rosalete;elizandra;armindo;evanir;otacilio;adelaide;germano;dora;aureo;jacir;helton;fausto;evaldo;marilaine;amauri;tarso;candida;idalina;zélia;quelen;nilsa;noemi;porto;araci;aracy;geny;eliezer;tomas;grasiela;fulvio;adilar;fabian;eleni;heraldo;ervino;assoc.;odilo;eladio;veridiana;adolfo;suely;cleverson;hendler;loreni;giselle;adelar;erno;jadir;alberi;marilise;valderez;shirley;alexis;epaminondas;giane;jandir;ruben;elci;albino;osmarino;peterson;erci;delmar;claudemir;angelina;cheila;valdeci;rosemeri;eron;avelino;danieli;isabela;heloi;gelci;guido;christiano;simoni;enedina;maribel;daisy;alcione;heliane;lucelia;dilnei;dolores;darli;olivio;premiere;benito;henri;erni;vandrea;nelio;suzete;loreci;giovanna;benta;silvano;serafim;nora;dagmar;marcel;alaor;zaira;hailton;marizete;grace;omar;ponciano;wilmar;nery;livio;genir;clodoveu;nayr;iuri;almedorino;joni;gecy;ulysses;elda;aguida;inajara;tanise;antero;cerutti;delma;lizete;emilia;sibele;mirna;banco;celeste;janira;rochelle;clodis;helga;onir;edenilson;ecila;rosaura;franciane;abraao;olivier;farol;vander;marinez;dejair;silviane;raphael;horacio;wilma;sadi;tanara;paloma;jaco;zilah;olavo;bolognesi;leone;associação;ramiro;cleide;claudecir;oswaldo;jacques".Split(';');

            //Optional: Initialize the log4net
            Log.LogManager.Initialize();

            Console.Write("Clear database? [Enter]: ");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                //Remove test data
                RemoveData();
                CreateSuportData();
            }

            Console.WriteLine("");
            Console.Write("Compare speed? [Enter]: ");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                DateTime begin = DateTime.Now;
                //Create the test data
                CreateDataMonty();
                DateTime end = DateTime.Now;

                Console.WriteLine("Monty Time: {0}", end - begin);

                begin = DateTime.Now;
                //Create the test data
                CreateDataSQL();
                end = DateTime.Now;

                Console.WriteLine("Regular SQL Time: {0}", end - begin);
            }

            Console.Title = "Monty.ActiveRecord Tester";

            Console.WriteLine("");
            Console.WriteLine("Test people A [Enter]: ");
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {

                Console.WriteLine("--- People starting with 'A': {0}", Person.CountAllWithLetter("a", MatchMode.Start));
                Console.WriteLine("--- People starting with 'B': {0}", Person.CountAllWithLetter("b", MatchMode.Start));
                Console.WriteLine("--- People starting with 'A' or 'B': {0}", Person.CountStartingWithAOrB());
                Console.WriteLine("--- People ending with 'A': {0}", Person.CountAllWithLetter("a", MatchMode.End));
                Console.WriteLine("--- People with 'A' anywhere: {0}", Person.CountAllWithLetter("a", MatchMode.Anywhere));

                Console.WriteLine("--- View Samples");
                Console.WriteLine("------ Most common name: {0}", PeopleNames.FindMostCommonName());
                Console.WriteLine("------ 5 Random names:");
                foreach (var item in PeopleNames.FindRandomNames(5))
                {
                    Console.WriteLine("--------- {0}", item);
                }
            }

            Console.ReadKey();
        }

        #region Data Methods

        /// <summary>
        /// Creates the suport data.
        /// </summary>
        static void CreateSuportData()
        {
            Job owner = CreateJob("Owner", "Owners gonna own");
            Job developer = CreateJob("Developer", "Developers gonna develop");
            Job tester = CreateJob("Tester", "Testers gonna test");

            Person pedro = CreatePerson("Pedro", new DateTime(1989, 11, 13), owner);
            for (int i = 1; i < 10; i++)
                AddDocument("Doc " + i.ToString(), pedro);

            Person maria = CreatePerson("Maria", new DateTime(1993, 1, 5), developer);
            Person juca = CreatePerson("Juca", null, developer);
            Person lisa = CreatePerson("Lisa", null, developer);
        }

        /// <summary>
        /// Creates the data via monty.activerecord.
        /// </summary>
        static void CreateDataMonty()
        {
            Job tester = Job.Find(3);

            for (int i = 0; i < 500; i++)
            {
                CreatePerson(RandomName(), null, tester);
                OutputTitle("Monty", i, 500);
            }
        }

        /// <summary>
        /// Creates the data via regular SQL.
        /// </summary>
        static void CreateDataSQL()
        {
            Job tester = Job.Find(3);

            for (int i = 0; i < 500; i++)
            {
                CreatePersonSQL(RandomName(), null, tester);
                OutputTitle("Regular SQL", i, 500);
            }
        }

        /// <summary>
        /// Removes the data.
        /// </summary>
        static void RemoveData()
        {
            Output("Removing data");
            ActiveRecordMaster.ExecuteNonQuery("DELETE FROM montyTDocument");
            ActiveRecordMaster.ExecuteNonQuery("DELETE FROM montyTPerson");
            ActiveRecordMaster.ExecuteNonQuery("DELETE FROM montyTJob");
            Output("Data removed");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a job.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Could not save</exception>
        static Job CreateJob(string name, string description)
        {
            Job job = new Job { Name = name, Description = description };

            if (!job.Save())
                throw new Exception("Could not save");

            return job;
        }

        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="currentJob">The current job.</param>
        /// <returns></returns>
        static Person CreatePerson(string name, Job currentJob)
        {
            Random rdn = new Random();
            return CreatePerson(name, new DateTime(1990, 1, 1).AddYears(rdn.Next(-5, 20)).AddMonths(rdn.Next(0, 12)).AddDays(rdn.Next(0, 31)), currentJob);
        }

        /// <summary>
        /// Creates a person using the Monty.ActiveRecord.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="birthday">The birthday.</param>
        /// <param name="currentJob">The current job.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Could not save</exception>
        static Person CreatePerson(string name, DateTime? birthday, Job currentJob)
        {
            Person person = new Person();

            person.Name = name;
            person.Birthday = birthday;
            person.CurrentJob = currentJob;

            if (!person.Save())
                throw new Exception("Could not save");

            return person;
        }

        /// <summary>
        /// Creates a person via SQL.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="birthday">The birthday.</param>
        /// <param name="currentJob">The current job.</param>
        /// <returns></returns>
        static Person CreatePersonSQL(string name, DateTime? birthday, Job currentJob)
        {
            string sql = string.Format("INSERT INTO montyTPerson (Name, Birthday, CurrentJob) VALUES ('{0}', null, {1});SELECT last_insert_id() AS 'Identity';", name, currentJob.Id);

            ActiveRecordMaster.ExecuteNonQuery(sql);

            return null;
        }

        /// <summary>
        /// Adds a document.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="owner">The owner.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Could not save</exception>
        static Document AddDocument(string name, Person owner)
        {
            Document document = new Document();

            document.Name = name;
            document.Owner = owner;

            if (!document.Save())
                throw new Exception("Could not save");

            return document;
        }

        #endregion

        #region Output Methods

        /// <summary>
        /// Outputs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        static void Output(String message)
        {
            Console.WriteLine("[{0}] - {1}", DateTime.Now.ToLongTimeString(), message);
        }

        /// <summary>
        /// Outputs the specified person.
        /// </summary>
        /// <param name="person">The person.</param>
        static void Output(Person person)
        {
            Console.WriteLine("--- [{0}] - {1}", person.Id, person.Name);
        }

        /// <summary>
        /// Outputs the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="current">The current.</param>
        /// <param name="max">The maximum.</param>
        static void OutputTitle(string title, int current, int max)
        {
            if (current == 0 || max == 0)
            {
                Console.Title = title;
                return;
            }

            Console.Title = String.Format("{0} - {1}%", title, ((100 * current) / max));
        }

        #endregion

        #region Helper Methods

        static string RandomName()
        {
            return ToTitleCase(RandomNames[new Random().Next(0, RandomNames.Length)]);
        }

        static string ToTitleCase(string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        #endregion
    }
}
