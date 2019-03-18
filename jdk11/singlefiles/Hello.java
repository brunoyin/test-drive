/*
 * Created on Fri Mar 15 2019
 *
 * Copyright (c) 2019 bruno.yin@gmail.com
 */


// java .\Hello.java

import picocli.CommandLine;
import picocli.CommandLine.Option;
import picocli.CommandLine.Parameters;

import java.io.File;

public class Hello implements Runnable 
{
    // public static void main( String[] args )
    // {
    //     System.out.println( "Hello World!" );
    // }
    @Option(names = { "-v", "--verbose" }, description = "Verbose mode. Helpful for troubleshooting. " +
    "Multiple -v options increase the verbosity.")
    private boolean[] verbose = new boolean[0];

    @Option(names = { "-h", "--help" }, usageHelp = true,
    description = "Displays this help message and quits.")
    private boolean helpRequested = false;

    @Parameters(arity = "1..*", paramLabel = "FILE", description = "File(s) to process.")
    private File[] inputFiles;

    public void run() {
        if (verbose.length > 0) {
            System.out.println(inputFiles.length + " files to process...");
        }
        if (verbose.length > 1) {
            for (File f : inputFiles) {
                System.out.println(f.getAbsolutePath());
            }
        }
    }

    public static void main(String[] args) {
        CommandLine.run(new Hello(), args);
    }
}