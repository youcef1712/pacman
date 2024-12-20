<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:sv="http://www.monjeu.com/jeuPacMan/sauvegarde"
                exclude-result-prefixes="sv">

    <!-- Template principal qui correspond à la racine du fichier XML source -->
    <xsl:template match="/">
        <Parties xmlns="http://www.monjeu.com/partie">
            <xsl:apply-templates select="sv:Sauvegarde/sv:Joueur"/>
        </Parties>
    </xsl:template>

    <!-- Template pour traiter chaque joueur -->
    <xsl:template match="sv:Joueur">
        <xsl:apply-templates select="sv:Parties/sv:Partie"/>
    </xsl:template>

    <!-- Template pour traiter chaque partie -->
    <xsl:template match="sv:Partie">
        <Partie xmlns="http://www.monjeu.com/partie">
            <NomJoueur>
                <xsl:value-of select="../../sv:Nom"/>
            </NomJoueur>
            <Date>
                <xsl:value-of select="sv:Date"/>
            </Date>
            <Score>
                <xsl:value-of select="sv:Score"/>
            </Score>
        </Partie>
    </xsl:template>
</xsl:stylesheet>
