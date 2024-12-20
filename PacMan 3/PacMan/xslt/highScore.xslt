<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:jeu="http://www.monjeu.com/jeuPacMan/sauvegarde" version="1.0">
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>

    <!-- Template principal -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Classement des Hi-Scores</title>
                <style>
                    body { font-family: Arial, sans-serif; }
                    table { width: 80%; margin: 20px auto; border-collapse: collapse; }
                    th, td { padding: 10px; text-align: left; border: 1px solid #ddd; }
                    th { background-color: #f4f4f4; }
                </style>
            </head>
            <body>
                <h1>Classement des Hi-Scores</h1>
                <table>
                    <tr>
                        <th>Classement</th>
                        <th>Nom du Joueur</th>
                        <th>Date</th>
                        <th>Score</th>
                    </tr>
                    <!-- Collecte des scores maximums -->
                    <xsl:for-each select="jeu:Sauvegarde/jeu:Joueur">
                        <!-- Trier par score maximum -->
                        <xsl:sort select="jeu:Parties/jeu:Partie[not(../jeu:Partie/jeu:Score > jeu:Score)]/jeu:Score" order="descending" data-type="number"/>

                        <!-- Calculer le score maximum et sa date associée -->
                        <xsl:variable name="maxScore" select="jeu:Parties/jeu:Partie[not(../jeu:Partie/jeu:Score > jeu:Score)]/jeu:Score"/>
                        <xsl:variable name="dateMaxScore" select="jeu:Parties/jeu:Partie[jeu:Score = $maxScore][1]/jeu:Date"/>

                        <tr>
                            <td><xsl:number/></td>
                            <td><xsl:value-of select="jeu:Nom"/></td>
                            <td><xsl:value-of select="$dateMaxScore"/></td>
                            <td><xsl:value-of select="$maxScore"/></td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>
