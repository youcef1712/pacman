<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:pc="http://www.monjeu.com/jeuPacMan">
    <xsl:output method="html" indent="yes" />

    <!-- Root template -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Résumé de l'État du Jeu Pac-Man</title>
                <link rel="stylesheet" type="text/css" href="../css/style2.css" />
            </head>
            <body>
                <div class="container">
                    <h1>Résumé de l'État du Jeu Pac-Man</h1>
                    <div class="data">
                        <div>
                            <span class="label">Etat du Jeu:</span>
                            <span class="value"><xsl:value-of select="//pc:etatJeu" /></span>
                        </div>
                        <div>
                            <span class="label">Score:</span>
                            <span class="value"><xsl:value-of select="//pc:score" /></span>
                        </div>
                        <div>
                            <span class="label">Vies:</span>
                            <span class="value"><xsl:value-of select="//pc:vies" /></span>
                        </div>
                        <div>
                            <span class="label">Temps Joué:</span>
                            <span class="value"><xsl:value-of select="//pc:tempsJoue" /></span>
                        </div>
                        <div>
                            <span class="label">Nom du Joueur:</span>
                            <span class="value"><xsl:value-of select="//pc:nomJoueur" /></span>
                        </div>
                        <div>
                            <span class="label">Distance Parcourue:</span>
                            <span class="value"><xsl:value-of select="//pc:distanceParcourue" /></span>
                        </div>
                        <div>
                            <span class="label">Distance Optimale:</span>
                            <span class="value"><xsl:value-of select="//pc:optimalDistance" /></span>
                        </div>
                        <div>
                            <span class="label">Temps Optimal:</span>
                            <span class="value"><xsl:value-of select="//pc:optimalTime" /></span>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
