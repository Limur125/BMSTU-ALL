drop table GameRating;
create table GameRating
(
	title varchar(200) primary key,
	console int check(console between 0 and 1),
	alcohol_reference int check(alcohol_reference between 0 and 1),
	animated_blood int check(animated_blood between 0 and 1),
	blood int check(blood between 0 and 1),
	blood_and_gore int check(blood_and_gore between 0 and 1),
	cartoon_violence int check(cartoon_violence between 0 and 1),
	crude_humor int check(crude_humor between 0 and 1),
	drug_reference int check(drug_reference between 0 and 1),
	fantasy_violence int check(fantasy_violence between 0 and 1),
	intense_violence int check(intense_violence between 0 and 1),
	[language] int check([language] between 0 and 1),
	lyrics int check(lyrics between 0 and 1),
	mature_humor int check(mature_humor between 0 and 1),
	mild_blood int check(mild_blood between 0 and 1),
	mild_cartoon_violence int check(mild_cartoon_violence between 0 and 1),
	mild_fantasy_violence int check(mild_fantasy_violence between 0 and 1),
	mild_language int check(mild_language between 0 and 1),
	mild_lyrics int check(mild_lyrics between 0 and 1),
	mild_suggestive_themes int check(mild_suggestive_themes between 0 and 1),
	mild_violence int check(mild_violence between 0 and 1),
	no_descriptors int check(no_descriptors between 0 and 1),
	nudity int check(nudity between 0 and 1),
	partial_nudity int check(partial_nudity between 0 and 1),
	sexual_content int check(sexual_content between 0 and 1),
	sexual_themes int check(sexual_themes between 0 and 1),
	simulated_gambling int check(simulated_gambling between 0 and 1),
	strong_janguage int check(strong_janguage between 0 and 1),
	strong_sexual_content int check(strong_sexual_content between 0 and 1),
	suggestive_themes int check(suggestive_themes between 0 and 1),
	use_of_alcohol int check(use_of_alcohol between 0 and 1),
	use_of_drugs_and_alcohol int check(use_of_drugs_and_alcohol between 0 and 1),
	violence int check(violence between 0 and 1),
	esrb_rating varchar(5) references Rating(rate)
);
bulk insert GameRating
from '\rate.txt'
with 
(
fieldterminator = '\t',
rowterminator = '\n'
);